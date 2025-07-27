using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct PlayerIndexCounterInitializationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<PlayerIndexCounter>())
        {
            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, new PlayerIndexCounter { CurrentIndex = 0 });
        }
    }

    public void OnUpdate(ref SystemState state) { }
    
}