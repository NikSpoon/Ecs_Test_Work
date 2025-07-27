using UnityEngine;

using Unity.Entities;
using Unity.Transforms;

public class CameraFollow : MonoBehaviour
{
    public Entity playerEntity;
    EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void LateUpdate()
    {
        if (playerEntity != Entity.Null && entityManager.Exists(playerEntity))
        {
            if (entityManager.HasComponent<LocalTransform>(playerEntity))
            {
                var playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

                // Получаем позицию игрока
                Vector3 playerPos = new Vector3(playerTransform.Position.x, playerTransform.Position.y, playerTransform.Position.z);

                // Задаём позицию камеры относительно игрока
                transform.position = playerPos + new Vector3(0, 5, -7);

                // Камера смотрит на игрока (Transform классический)
                transform.LookAt(playerPos);
            }
        }
    }
}
