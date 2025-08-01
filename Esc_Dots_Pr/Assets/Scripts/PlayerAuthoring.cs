using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct Player : IComponentData
{

}

[DisallowMultipleComponent]
public class PlayerAuthoring : MonoBehaviour
{
    [HideInInspector]
    public Entity entity;

    class Baker : Baker<PlayerAuthoring>
    {

        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);

            AddComponent(entity, new AnimationStateData
            {
                Current = AnimationState.Idle // или любой стартовый стейт
            });

            authoring.entity = entity;
        }
    }
}
