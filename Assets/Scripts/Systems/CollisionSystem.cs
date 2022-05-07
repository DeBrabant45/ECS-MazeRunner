using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

public partial class CollisionSystem : SystemBase
{
    private struct CollisionSystemJob : ICollisionEventsJob
    {
        public BufferFromEntity<CollisionBuffer> Collisions;
        public void Execute(CollisionEvent collisionEvent)
        {
            if (Collisions.HasComponent(collisionEvent.EntityA))
            {
                Collisions[collisionEvent.EntityA].Add(
                    new CollisionBuffer { Entity = collisionEvent.EntityB });
            }
            if (Collisions.HasComponent(collisionEvent.EntityB))
            {
                Collisions[collisionEvent.EntityB].Add(
                    new CollisionBuffer { Entity = collisionEvent.EntityA });
            }
        }
    }

    private struct TriggerSystemJob : ITriggerEventsJob
    {
        public BufferFromEntity<TriggerBuffer> Triggers;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (Triggers.HasComponent(triggerEvent.EntityA))
            {
                Triggers[triggerEvent.EntityA].Add(
                    new TriggerBuffer { Entity = triggerEvent.EntityB });
            }
            if (Triggers.HasComponent(triggerEvent.EntityB))
            {
                Triggers[triggerEvent.EntityB].Add(
                    new TriggerBuffer { Entity = triggerEvent.EntityA });
            }
        }
    }

    protected override void OnUpdate()
    {
        var simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;
        ClearCollisionBuffers();
        ClearTriggerBuffers();
        AddCollisionJobs(simulation);
        AddTriggerJobs(simulation);
    }

    private void AddTriggerJobs(ISimulation simulation)
    {
        var triggerJob = new TriggerSystemJob()
        {
            Triggers = GetBufferFromEntity<TriggerBuffer>(),
        }.Schedule(simulation, this.Dependency);
        triggerJob.Complete();
    }

    private void AddCollisionJobs(ISimulation simulation)
    {
        var collisionJob = new CollisionSystemJob()
        {
            Collisions = GetBufferFromEntity<CollisionBuffer>(),
        }.Schedule(simulation, this.Dependency);
        collisionJob.Complete();
    }

    private void ClearTriggerBuffers()
    {
        Entities.ForEach((DynamicBuffer<TriggerBuffer> triggers) =>
        {
            triggers.Clear();
        }).Run();
    }

    private void ClearCollisionBuffers()
    {
        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisions) =>
        {
            collisions.Clear();
        }).Run();
    }
}