using Unity.Entities;

public partial struct ServerBoneAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (animState, entity) in SystemAPI.Query<RefRO<AnimationStateData>>().WithEntityAccess())
        {
            // Здесь логика: какая кость, какое положение
            var boneSync = new BoneSyncData
            {
              
            };

            state.EntityManager.SetComponentData(entity, boneSync);
        }
    }
}

