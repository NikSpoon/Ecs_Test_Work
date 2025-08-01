using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
[BurstCompile]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        var deltaTime = SystemAPI.Time.DeltaTime;
        var speedMultiplier = 4f;
        var rotationSpeed = 10f;

        foreach (var (input, transform, entity) in SystemAPI.Query<RefRO<PlayerInput>, RefRW<LocalTransform>>()
                     .WithEntityAccess().WithAll<Simulate>())
        {
            var moveInput = new float2(input.ValueRO.Horizontal, input.ValueRO.Vertical);
            float moveSpeed = math.length(moveInput);

            if (moveSpeed > 0.01f)
            {
                var moveDir = math.normalizesafe(moveInput);
                var movement = moveDir * moveSpeed * speedMultiplier * deltaTime;

                transform.ValueRW.Position += new float3(movement.x, 0, movement.y);

                var forward = new float3(moveDir.x, 0, moveDir.y);
                var targetRot = quaternion.LookRotationSafe(forward, math.up());
                transform.ValueRW.Rotation = math.slerp(transform.ValueRW.Rotation, targetRot, deltaTime * rotationSpeed);
            }

        }
    }
}
