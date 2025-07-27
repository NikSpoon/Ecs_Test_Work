using Unity.Entities;
using UnityEngine;

public class PlayerAnimatorAuthoring : MonoBehaviour
{
    public Animator animator;
}
public class PlayerAnimatorBaker : Baker<PlayerAnimatorAuthoring>
{
    public override void Bake(PlayerAnimatorAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        // Храним ссылку на Animator в ECS — как объект
        AddComponentObject(entity, authoring.animator);

        // Можно также добавить тэг или ECS-компонент, если нужно
        AddComponent<PlayerAnimatorTag>(entity);
    }
}
public struct PlayerAnimatorTag : IComponentData { }

public partial class PlayerAnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<PlayerAnimatorTag>()
            .ForEach((Entity entity, in PlayerInput input) =>
            {
                var animator = EntityManager.GetComponentObject<Animator>(entity);
                if (input.isRunning)
                    animator.SetBool("IsRunning", true);
                else
                    animator.SetBool("IsRunning", false);
            }).WithoutBurst().Run(); // Без Burst, т.к. работаем с объектом
    }
}