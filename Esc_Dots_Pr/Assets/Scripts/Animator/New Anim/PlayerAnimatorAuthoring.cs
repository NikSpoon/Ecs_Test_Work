using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAnimatorAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerAnimatorAuthoring>
    {
        public override void Bake(PlayerAnimatorAuthoring authoring)
        {

            var animator = authoring.GetComponent<PlayerAnimator>();
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            animator.Animator = animator.GetComponent<Animator>();
            AddComponentObject(entity, animator);
        }

    }

}
