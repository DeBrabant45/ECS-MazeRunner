using Unity.Entities;
using Unity.Transforms;

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
        var endSimBuffer = endSimBufferSystem.CreateCommandBuffer();
        Entities.ForEach((Entity entity, ref Kill kill, in Translation tranform, in Rotation rotation) =>
        {
            kill.Timer -= deltaTime;
            if (kill.Timer <= 0)
            {
                if (HasComponent<OnKill>(entity))
                {
                    var onKill = GetComponent<OnKill>(entity);
                    AudioManager.Instance.PlaySFXRequest(onKill.SFXName.ToString());
                    GameManager.Instance.AddScore(onKill.Point);
                    if (EntityManager.Exists(onKill.SpawnPrefab))
                    {
                        var sapwnedEntity = endSimBuffer.Instantiate(onKill.SpawnPrefab);
                        endSimBuffer.AddComponent(sapwnedEntity, tranform);
                        endSimBuffer.AddComponent(sapwnedEntity, rotation);
                    }
                }
                endSimBuffer.DestroyEntity(entity);
            }
        }).WithoutBurst().Run();
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