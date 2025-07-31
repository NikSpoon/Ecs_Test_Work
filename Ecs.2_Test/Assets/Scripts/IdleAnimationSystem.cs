using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct IdleAnimationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BoneAnimationTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (transform, tag) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BoneAnimationTag>>())
        {
            float angle = math.sin(time * tag.ValueRO.Speed) * 10f;
            transform.ValueRW.Rotation = quaternion.EulerXYZ(0f, 0f, math.radians(angle));
        }
    }
}