using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct PlayerSpawner : IComponentData
{
    public Entity Player1;
    public Entity Player2;
}

[DisallowMultipleComponent]
public class PlayerSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _playerPewf1;

    [SerializeField] private GameObject _playerPewf2;


    class Baker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            PlayerSpawner component = default(PlayerSpawner);

            component.Player1 = GetEntity(authoring._playerPewf1, TransformUsageFlags.Dynamic);
            component.Player2 = GetEntity(authoring._playerPewf2, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, component);

        }
    }
}
