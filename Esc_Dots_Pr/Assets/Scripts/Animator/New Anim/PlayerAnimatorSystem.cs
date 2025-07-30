using Unity.Entities;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerAnimatorSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        Debug.Log(1);
        foreach (var (input, entity) in SystemAPI.Query<RefRO<PlayerInput>>().WithEntityAccess())
        {
            Debug.Log(2);
            if (!state.EntityManager.HasComponent<PlayerAnimator>(entity))
                continue;
            Debug.Log(3);
            var animator = state.EntityManager.GetComponentObject<PlayerAnimator>(entity);
            if (animator != null && animator.Animator != null)
            {
                Debug.Log(4);
                animator.Animator.SetBool("IsRunning", input.ValueRO.IsRunning);
          
            }
        }
    }
}
