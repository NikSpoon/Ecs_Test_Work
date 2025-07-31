using Unity.Entities;
using UnityEngine;

public class BoneAnimationAuthoring : MonoBehaviour
{
    public float Speed = 1f;

    class Baker : Baker<BoneAnimationAuthoring>
    {
        public override void Bake(BoneAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BoneAnimationTag { Speed = authoring.Speed });
        }
    }
}

public struct BoneAnimationTag : IComponentData
{
    public float Speed;
}
