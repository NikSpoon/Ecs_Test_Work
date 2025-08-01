using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class FullCharacterBoneAuthoring : MonoBehaviour
{
    public Transform RootBone; // Корень иерархии костей
}

public class FullCharacterBoneBaker : Baker<FullCharacterBoneAuthoring>
{
    public override void Bake(FullCharacterBoneAuthoring authoring)
    {
        var characterRoot = GetEntity(TransformUsageFlags.Dynamic);
        var root = authoring.RootBone != null ? authoring.RootBone : authoring.transform;

        var allBones = root.GetComponentsInChildren<Transform>(true);

        foreach (var boneTransform in allBones)
        {
            // Убедись, что кость — child entity
            var boneEntity = GetEntity(boneTransform, TransformUsageFlags.Dynamic);

            AddComponent(boneEntity, new BoneTag { BoneName = new FixedString64Bytes(boneTransform.name) });
            AddComponent(boneEntity, new BoneOwner { CharacterRoot = characterRoot });
            AddComponent<BoneSyncTag>(boneEntity);

            // 👇 Чтобы была связь между Transform и Entity
            AddComponentObject(boneEntity, boneTransform);
        }
    }
}

public struct BoneTag : IComponentData
{
    public FixedString64Bytes BoneName;
}

public struct BoneOwner : IComponentData
{
    public Entity CharacterRoot;
}

public struct BoneSyncTag : IComponentData { }