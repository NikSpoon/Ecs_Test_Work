using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

public class CharacterAuthoring : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;

    // (можно автоматом собирать кости по иерархии, если надо)
    public Transform rootBone;
    public Transform[] bones;

    class Baker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // ✅ Сохраняем анимационный стейт
            AddComponent(entity, new AnimationStateData
            {
                Current = AnimationState.Idle
            });

            // ✅ Добавляем ссылку на SkinnedMeshRenderer как OBJECT
            AddComponentObject(entity, authoring.skinnedMeshRenderer);

            // ✅ Добавляем кости вручную — если надо
            foreach (Transform bone in authoring.bones)
            {
                var boneEntity = GetEntity(bone, TransformUsageFlags.Dynamic);

                AddComponent(boneEntity, new BoneTag
                {
                    BoneName = bone.name
                });

                AddComponent(boneEntity, new AnimationStateData
                {
                    Current = AnimationState.Idle
                });
            }
        }
    }
}
