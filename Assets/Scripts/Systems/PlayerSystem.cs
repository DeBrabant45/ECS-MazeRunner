using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public partial class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        AddMovement();
        AddPowerUp();
    }

    private void AddPowerUp()
    {
        var deltaTime = Time.DeltaTime;
        var endSimBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        Entities
            .WithAll<Player>()
            .ForEach((Entity entity, ref Health health, ref PowerPill pill, ref Damage damage) =>
            {
                damage.Amount = 100;
                pill.Timer -= deltaTime;
                health.InvincibleTimer = pill.Timer;
                if (pill.Timer <= 0)
                {
                    endSimBufferSystem.RemoveComponent<PowerPill>(entity);
                    damage.Amount = 0;
                }

            }).Schedule();
    }

    private void AddMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Entities
            .WithAll<Player>()
            .ForEach((ref Movable movable) =>
            {
                movable.Direction = new float3(horizontalInput, 0, verticalInput);
            }).Schedule();
    }
}