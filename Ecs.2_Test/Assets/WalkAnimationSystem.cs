using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct WalkAnimationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BoneAnimationTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;
        var entityManager = state.EntityManager;

        foreach (var (transform, tag, entity) in SystemAPI
                     .Query<RefRW<LocalTransform>, RefRO<BoneAnimationTag>>()
                     .WithEntityAccess())
        {
            float angle = math.sin(time * tag.ValueRO.Speed) * 30f;

            // Здесь мы получаем имя сущности безопасно через entityManager
            var name = entityManager.GetName(entity);
            if (name.Contains(".l"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(math.radians(angle), 0f, 0f);
            }
            else if (name.Contains(".r"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(math.radians(-angle), 0f, 0f);
            }
        }
    }
}
