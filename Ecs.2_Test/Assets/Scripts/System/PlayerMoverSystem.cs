using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

partial struct PlayerMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (
            (RefRW<LocalTransform> localTransform,
             RefRO<MoveSpeed> moveSpeed,
             RefRW<PhysicsVelocity> physicsVelocoty)
        in SystemAPI.Query
             <RefRW<LocalTransform>,
             RefRO<MoveSpeed>,
             RefRW<PhysicsVelocity>>())
        {

            float3 targetPosition = localTransform.ValueRW.Position + new float3(10, 0, 0);
            float3 moveDirection = targetPosition - localTransform.ValueRW.Position;

            moveDirection = math.normalize(moveDirection);

            localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());

           physicsVelocoty.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.value;
           physicsVelocoty.ValueRW.Angular = float3.zero;
           //localTransform.ValueRW.Position = localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }
    }

}
