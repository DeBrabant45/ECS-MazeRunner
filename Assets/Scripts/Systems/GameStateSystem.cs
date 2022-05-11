using Unity.Entities;
using UnityEngine;

[AlwaysUpdateSystem]
public partial class GameStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var pelletQuery = GetEntityQuery(ComponentType.ReadOnly<Pellet>());
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<Player>());
        if (pelletQuery.CalculateEntityCount() <= 0)
        {
            GameManager.Instance.Win();
        }
        if (playerQuery.CalculateEntityCount() <= 0)
        {
            GameManager.Instance.Lose();
        }
    }
}