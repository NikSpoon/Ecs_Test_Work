using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct JumpAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (transform, tag, stateData) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<BoneTag>, RefRO<AnimationStateData>>())
        {
            if (stateData.ValueRO.Current != AnimationState.Jump)
                continue;

            var pos = transform.ValueRW.Position;
            pos.y = math.abs(math.sin(time * 4f)) * 0.2f;
            transform.ValueRW.Position = pos;
        }
    }
}
