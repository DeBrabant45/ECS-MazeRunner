using Unity.Entities;
using Unity.Transforms;

public partial class SpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Spawner spawner, in Translation transform, in Rotation rotation) =>
        {
            if (!EntityManager.Exists(spawner.SpawnObject))
            {
                spawner.SpawnObject = EntityManager.Instantiate(spawner.Prefab);
                EntityManager.SetComponentData(spawner.SpawnObject, transform);
                EntityManager.SetComponentData(spawner.SpawnObject, rotation);
            }
        }).WithStructuralChanges().Run();
    }
}