using Unity.Entities;
using Unity.NetCode;

public class IdleAnimationAuthoring : UnityEngine.MonoBehaviour
{
    public float TimeOffset = 0f;

    class Baker : Baker<IdleAnimationAuthoring>
    {
        public override void Bake(IdleAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new IdleAnimationData
            {
                TimeOffset = authoring.TimeOffset
            });
        }
    }
}

