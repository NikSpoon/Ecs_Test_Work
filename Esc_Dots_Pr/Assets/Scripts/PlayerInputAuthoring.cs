using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct PlayerInput : IInputComponentData
{
    public bool isRunning;
    public int Horizontal;
    public int Vertical;
}

[DisallowMultipleComponent]
public class PlayerInputAuthoring : MonoBehaviour
{
    class CubeInputBaking : Baker<PlayerInputAuthoring>
    {
        public override void Bake(PlayerInputAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerInput>(entity);
        }
    }
}

[UpdateInGroup(typeof(GhostInputSystemGroup))]
public partial struct SamplePlayerInput : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>();
        state.RequireForUpdate<PlayerSpawner>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var playerInput in SystemAPI.Query<RefRW<PlayerInput>>().WithAll<GhostOwnerIsLocal>())
        {
            playerInput.ValueRW = default;
            if (Input.GetKey(KeyCode.A))
                playerInput.ValueRW.Horizontal -= 1;
            if (Input.GetKey(KeyCode.D))
                playerInput.ValueRW.Horizontal += 1;
            if (Input.GetKey(KeyCode.S))
                playerInput.ValueRW.Vertical -= 1;
            if (Input.GetKey(KeyCode.W))
                playerInput.ValueRW.Vertical += 1;

            playerInput.ValueRW.isRunning =  playerInput.ValueRW.Horizontal != 0 || playerInput.ValueRW.Vertical != 0;
        }
    }
}

