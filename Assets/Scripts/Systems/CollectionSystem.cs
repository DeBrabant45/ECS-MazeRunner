using Unity.Entities;

public partial class CollectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var endSimBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        Entities
            .WithAll<Player>()
            .ForEach((Entity playerEntity, DynamicBuffer<TriggerBuffer> triggers) =>
            {
                for (int i = 0; i < triggers.Length; i++)
                {
                    var entity = triggers[i].Entity;
                    if (HasComponent<Collectable>(entity) && !HasComponent<Kill>(entity))
                    {
                        endSimBufferSystem.AddComponent(entity, new Kill { Timer = 0 });
                    }

                    if (HasComponent<PowerPill>(entity) && !HasComponent<Kill>(entity))
                    {
                        endSimBufferSystem.AddComponent(playerEntity, GetComponent<PowerPill>(entity));
                        endSimBufferSystem.AddComponent(entity, new Kill { Timer = 0 });
                    }
                }
            }).Run();
    }
}