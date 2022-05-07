using Unity.Entities;

public partial class DamageSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        EntityTakeDamage();
        AddKillComponent(deltaTime);
        DestroyEntity(deltaTime);
    }

    private void EntityTakeDamage()
    {
        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffers, ref Health health) =>
        {
            for (int i = 0; i < collisionBuffers.Length; i++)
            {
                if (health.InvincibleTimer <= 0 && HasComponent<Damage>(collisionBuffers[i].Entity))
                {
                    health.Amount -= GetComponent<Damage>(collisionBuffers[i].Entity).Amount;
                    health.InvincibleTimer = 1f;
                }
            }
        }).Schedule();
    }

    private void DestroyEntity(float deltaTime)
    {
        var endSimBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var endSimBuffer = endSimBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Kill kill) =>
        {
            kill.Timer -= deltaTime;
            if (kill.Timer <= 0)
            {
                endSimBuffer.DestroyEntity(entityInQueryIndex, entity);
            }
        }).Schedule();
        endSimBufferSystem.AddJobHandleForProducer(this.Dependency);
    }

    private void AddKillComponent(float deltaTime)
    {
        Entities
            .WithNone<Kill>()
            .ForEach((Entity entity, ref Health health) =>
            {
                health.InvincibleTimer -= deltaTime;
                if (health.Amount <= 0)
                {
                    EntityManager.AddComponentData(entity, new Kill { Timer = health.KillTimer });
                }
            }).WithStructuralChanges().Run();
    }
}