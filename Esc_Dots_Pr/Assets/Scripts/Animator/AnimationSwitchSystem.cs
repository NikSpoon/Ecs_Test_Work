using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct AnimationStateSwitcher : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var animState in SystemAPI.Query<RefRW<AnimationStateData>>())
        {
            float t = (float)SystemAPI.Time.ElapsedTime;
            int index = 0;
            animState.ValueRW.Current = (AnimationState)index;
        }
    }
}
