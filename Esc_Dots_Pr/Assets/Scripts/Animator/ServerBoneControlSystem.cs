using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.NetCode;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct ServerBoneControlSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (buffer, entity) in SystemAPI
              .Query<DynamicBuffer<LinkedEntityGroup>>()
              .WithAll<PlayerInfo>()
              .WithEntityAccess())
        {
            foreach (var linked in buffer)
            {
                if (!SystemAPI.HasComponent<BoneTag>(linked.Value))
                    continue;

                var boneName = SystemAPI.GetComponent<BoneTag>(linked.Value).BoneName;

                if (boneName == "mixamorig:Spine1")
                {
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(linked.Value);
                    transform.ValueRW = transform.ValueRW.RotateY(0.01f);
                }
            }
        }

    }
}
