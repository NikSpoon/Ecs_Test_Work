using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct AttackAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, tag, stateData) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<BoneTag>, RefRO<AnimationStateData>>())
        {
            if (stateData.ValueRO.Current != AnimationState.Attack)
                continue;

            var bone = tag.ValueRO.BoneName.ToString();
            if (bone.Contains("Spine"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(0.3f, 0, 0);
            }
        }
    }
}
