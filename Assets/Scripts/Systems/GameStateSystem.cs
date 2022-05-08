using Unity.Entities;
using UnityEngine;

[AlwaysUpdateSystem]
public partial class GameStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var pelletQuery = GetEntityQuery(ComponentType.ReadOnly<Pellet>());
        Debug.Log(pelletQuery.CalculateEntityCount());
    }
}