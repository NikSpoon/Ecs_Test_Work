using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct SyncBoneTransformSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
  
    }
}