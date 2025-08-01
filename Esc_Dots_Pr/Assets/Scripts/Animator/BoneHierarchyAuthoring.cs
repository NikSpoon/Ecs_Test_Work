using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;
using Unity.NetCode;
using Unity.Mathematics;

public class BoneBaker : Baker<BoneHierarchyAuthoring>
{
    public override void Bake(BoneHierarchyAuthoring authoring)
    {
        if (authoring.CharacterRoot == null)
        {
            Debug.LogWarning($"[BoneBaker] CharacterRoot not assigned on {authoring.gameObject.name}");
            return;
        }

        var boneEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        var rootEntity = GetEntity(authoring.CharacterRoot, TransformUsageFlags.Dynamic);

        var boneName = string.IsNullOrEmpty(authoring.BoneNameOverride)
            ? authoring.gameObject.name
            : authoring.BoneNameOverride;

        AddComponent(boneEntity, new BoneTag
        {
            BoneName = new FixedString64Bytes(boneName)
        });

        AddComponent(boneEntity, new BoneOwner
        {
            CharacterRoot = rootEntity
        });

        AddComponent<BoneSyncTag>(boneEntity);
        AddComponent<LocalTransform>(boneEntity);
        AddComponentObject(boneEntity, authoring.transform);
    }
}

public class BoneHierarchyAuthoring : MonoBehaviour
{
    public string BoneNameOverride;
    public GameObject CharacterRoot;
}
public struct BoneTag : IComponentData
{
    public FixedString64Bytes BoneName;
}

public struct BoneOwner : IComponentData
{
    public Entity CharacterRoot;
}

[GhostComponent(PrefabType = GhostPrefabType.All)]
public struct BoneSyncTag : IComponentData { }

