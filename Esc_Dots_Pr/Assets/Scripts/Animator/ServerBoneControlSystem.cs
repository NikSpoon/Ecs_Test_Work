using Unity.Entities;

public partial struct ServerBoneAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (animState, entity) in SystemAPI.Query<RefRO<AnimationStateData>>().WithEntityAccess())
        {
            // Здесь логика: какая кость, какое положение
            var boneSync = new BoneSyncData
            {
              
            };

<<<<<<< HEAD
            state.EntityManager.SetComponentData(entity, boneSync);
=======
                var boneName = SystemAPI.GetComponent<BoneTag>(linked.Value).BoneName;

                if (boneName == "mixamorig:Spine1")
                {
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(linked.Value);
                   
                }
            }
>>>>>>> origin/New
        }
    }
}

