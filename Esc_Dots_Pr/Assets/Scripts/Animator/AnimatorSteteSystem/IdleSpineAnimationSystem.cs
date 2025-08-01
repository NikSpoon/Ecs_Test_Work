using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct IdleAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;
        float sway = math.sin(time * 1.5f);

        foreach (var (transform, tag, animState) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<BoneTag>, RefRO<AnimationStateData>>())
        {
            if (animState.ValueRO.Current != AnimationState.Idle)
                continue;

            var bone = tag.ValueRO.BoneName.ToString();

            if (bone.Contains("Spine1") || bone.Contains("Spine2"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(sway * 0.02f, 0, 0);
            }
            else if (bone.Contains("LeftArm") || bone.Contains("RightArm"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(sway * 0.04f, 0, 0);
            }
            else if (bone.Contains("LeftForeArm") || bone.Contains("RightForeArm"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(sway * 0.03f, 0, 0);
            }
            else if (bone.Contains("Head"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(sway * 0.015f, 0, 0);
            }
            else if (bone.Contains("LeftFoot") || bone.Contains("RightFoot"))
            {
                transform.ValueRW.Rotation = quaternion.EulerXYZ(0, 0, sway * 0.005f);
            }
          //  UnityEngine.Debug.Log($"Bone {bone}: Rotation = {transform.ValueRW.Rotation.value}");
        }
    }
}
