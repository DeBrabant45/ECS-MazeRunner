using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[AlwaysUpdateSystem]
public partial class GameStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var pelletQuery = GetEntityQuery(ComponentType.ReadOnly<Pellet>());
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<Player>());
        var pelletCount = pelletQuery.CalculateEntityCount();
        if (playerQuery.CalculateEntityCount() > 0)
        {
            GameManager.Instance.SetPelletCount(pelletCount);
        }
        if (pelletCount <= 0)
        {
            RemovePhysicsOnWin();
        }
        SetLoseState();
    }

    private void RemovePhysicsOnWin()
    {
        Entities
            .WithAll<PhysicsVelocity>()
            .ForEach((Entity entity) =>
            {
                EntityManager.RemoveComponent<PhysicsVelocity>(entity);
            }).WithStructuralChanges().Run();
    }

    private void SetLoseState()
    {
        var enemyQuery = GetEntityQuery(ComponentType.ReadOnly<Enemy>());
        Entities
            .WithAll<Player, Movable>()
            .ForEach((Entity entity, in Kill kill) =>
            {
                EntityManager.RemoveComponent<Movable>(entity);
                EntityManager.RemoveComponent<PhysicsVelocity>(entity);
                GameManager.Instance.LoseLife();
                var enemyArray = enemyQuery.ToEntityArray(Allocator.TempJob);
                foreach (var enenmyEnity in enemyArray)
                {
                    EntityManager.AddComponentData(enenmyEnity, kill);
                    EntityManager.RemoveComponent<Movable>(enenmyEnity);
                    EntityManager.RemoveComponent<PhysicsVelocity>(enenmyEnity);
                }
                enemyArray.Dispose();
            }).WithStructuralChanges().Run();
    }
}