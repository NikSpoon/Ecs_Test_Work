using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGhostAuthoring : MonoBehaviour
{
    public float speedValue;

    class Baker : Baker<PlayerGhostAuthoring>
    {
        public override void Bake(PlayerGhostAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
            AddComponent<MoveSpeed>(entity);
        }
    }
}
public struct Player : IComponentData
{
}