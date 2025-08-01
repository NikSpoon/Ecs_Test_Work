using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;

public class BoneAuthoring : MonoBehaviour
{
    [Tooltip("Оставь пустым, чтобы использовать имя GameObject")]
    public string OverrideBoneName;
}

public class BoneAuthoringBaker : Baker<BoneAuthoring>
{
    public override void Bake(BoneAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        var boneName = string.IsNullOrEmpty(authoring.OverrideBoneName)
            ? authoring.gameObject.name
            : authoring.OverrideBoneName;

        AddComponent(entity, new BoneTag
        {
            BoneName = new FixedString64Bytes(boneName)
        });

        AddComponent(entity, new BoneSyncData
        {
            LocalPosition = float3.zero,
            LocalRotation = quaternion.identity
        });

        // Добавляем Transform для доступа к MonoBehaviour
        AddComponentObject(entity, authoring.transform);

        // Не забудь: BoneSyncBehaviour должен быть на этом же объекте
        var syncBehaviour = authoring.GetComponent<BoneSyncBehaviour>();
        if (syncBehaviour != null)
        {
            AddComponentObject(entity, syncBehaviour);
        }
    }
}
public struct BoneTag : IComponentData
{
    public FixedString64Bytes BoneName;
}

public struct BoneSyncData : IComponentData
{
    public float3 LocalPosition;
    public quaternion LocalRotation;
}