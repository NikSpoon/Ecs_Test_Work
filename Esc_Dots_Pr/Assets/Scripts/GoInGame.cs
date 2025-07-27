
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;



[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
[UpdateInGroup(typeof(InitializationSystemGroup))]
[CreateAfter(typeof(RpcSystem))]
public partial struct SetRpcSystemDynamicAssemblyListSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        SystemAPI.GetSingletonRW<RpcCollection>().ValueRW.DynamicAssemblyList = true;
        state.Enabled = false;
    }
}

public struct GoInGameRequest : IRpcCommand
{
    public FixedString64Bytes PlayerNameRequest;
}
public struct PlayerInfo : IComponentData
{
    public FixedString64Bytes PlayerName;
    public int SelectedCharacterIndex;
}
public struct PlayerIndexCounter : IComponentData
{
    public int CurrentIndex;
}
[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
public partial struct GoInGameClientSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
        state.RequireForUpdate<PlayerSpawner>();

        var builder = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<NetworkId>()
            .WithNone<NetworkStreamInGame>();
        state.RequireForUpdate(state.GetEntityQuery(builder));
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var playerPrefix = new FixedString64Bytes("Player");

        foreach (var (id, entity) in
                 SystemAPI.Query<RefRO<NetworkId>>()
                          .WithEntityAccess()
                          .WithNone<NetworkStreamInGame>())
        {
            commandBuffer.AddComponent<NetworkStreamInGame>(entity);

            var req = commandBuffer.CreateEntity();
            var name = playerPrefix;

            commandBuffer.AddComponent(req, new GoInGameRequest
            {
                PlayerNameRequest = name
            });

            commandBuffer.AddComponent(req, new SendRpcCommandRequest
            {
                TargetConnection = entity
            });
        }

        commandBuffer.Playback(state.EntityManager);
    }
}

[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct GoInGameServerSystem : ISystem
{
    private ComponentLookup<NetworkId> networkIdFromEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerSpawner>();
        state.RequireForUpdate<PlayerIndexCounter>();

        var builder = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<GoInGameRequest>()
            .WithAll<ReceiveRpcCommandRequest>();
        state.RequireForUpdate(state.GetEntityQuery(builder));

        networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency.Complete();

        networkIdFromEntity.Update(ref state);

        var spawner = SystemAPI.GetSingleton<PlayerSpawner>();
        var playerIndexCounter = SystemAPI.GetSingletonRW<PlayerIndexCounter>();

        var worldName = new FixedString32Bytes(state.WorldUnmanaged.Name);
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (rpc, rpcEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
                                                        .WithAll<GoInGameRequest>()
                                                        .WithEntityAccess())
        {
            var goInGameRequest = SystemAPI.GetComponent<GoInGameRequest>(rpcEntity);
            var connection = rpc.ValueRO.SourceConnection;

            commandBuffer.AddComponent<NetworkStreamInGame>(connection);
            var networkId = networkIdFromEntity[connection];

            playerIndexCounter.ValueRW.CurrentIndex++;
            int assignedIndex = playerIndexCounter.ValueRW.CurrentIndex;

            Entity prefabToSpawn;
            if (assignedIndex == 1)
                prefabToSpawn = spawner.Player1;
            else if (assignedIndex == 2)
                prefabToSpawn = spawner.Player2;
            else
            {
#if !UNITY_BURST
                UnityEngine.Debug.LogWarning($"No prefab for player index {assignedIndex}, spawning default.");
#endif
                prefabToSpawn = default;
            }
#if !UNITY_BURST
            state.EntityManager.GetName(prefabToSpawn, out var prefabName);
            UnityEngine.Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game, spawning a Ghost '{prefabName}' with SelectedCharacterIndex={assignedIndex}!");
#endif  
            var player = commandBuffer.Instantiate(prefabToSpawn);

            commandBuffer.SetComponent(player, new GhostOwner { NetworkId = networkId.Value });

            commandBuffer.AddComponent(player, new PlayerInfo
            {
                PlayerName = goInGameRequest.PlayerNameRequest,
                SelectedCharacterIndex = assignedIndex
            });

            commandBuffer.AppendToBuffer(connection, new LinkedEntityGroup { Value = player });
            commandBuffer.DestroyEntity(rpcEntity);
        }

        commandBuffer.Playback(state.EntityManager);

    }
}