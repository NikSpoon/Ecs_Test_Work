using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BoneSyncBehaviour : MonoBehaviour
{
    public Entity myEntity;
    public EntityManager EntityManager;
    public string boneName;

    private void Start()
    {
        boneName = gameObject.name;
    }


    void LateUpdate()
    {
        if (!EntityManager.Exists(myEntity)) return;

        if (EntityManager.HasComponent<BoneSyncData>(myEntity))
        {
            var data = EntityManager.GetComponentData<BoneSyncData>(myEntity);
            transform.localPosition = data.LocalPosition;
            transform.localRotation = data.LocalRotation;
        }
    }
}

