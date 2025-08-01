using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct WalkAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (transform, tag, stateData) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<BoneTag>, RefRO<AnimationStateData>>())
        {
            if (stateData.ValueRO.Current != AnimationState.Walk)
                continue;

            var bone = tag.ValueRO.BoneName.ToString();
            float angle = math.sin(time * 5f) * 0.3f;

            // Только пример: качай ноги и руки
            if (bone.Contains("LeftLeg") || bone.Contains("RightLeg"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(angle, 0, 0);
            }
            else if (bone.Contains("LeftArm") || bone.Contains("RightArm"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(-angle, 0, 0);
            }
        }
    }
}
