using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySystem : SystemBase
{
    private Unity.Mathematics.Random _random;

    public EnemySystem()
    {
        _random = new Unity.Mathematics.Random(1234);
    }

    protected override void OnUpdate()
    {
        _random.NextInt();
        AddMovement();
    }

    private void AddMovement()
    {
        var randomTemp = _random;
        var validDirection = new NativeList<float3>(Allocator.Temp);
        var movementRay = new MovementRayCast { PhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld };
        Entities.ForEach((ref Movable move, ref Enemy enemy, in Translation transform) =>
        {
            bool hitWall = movementRay.CheckRay(transform.Value, move.Direction, move.Direction);
            if (math.distance(transform.Value, enemy.PreviousCell) > 0.9f || hitWall)
            {
                enemy.PreviousCell = math.round(transform.Value);
                var validDirection = new NativeList<float3>(Allocator.Temp);
                if (!movementRay.CheckRay(transform.Value, new float3(0, 0, -1), move.Direction))
                    validDirection.Add(new float3(0, 0, -1));
                if (!movementRay.CheckRay(transform.Value, new float3(0, 0, 1), move.Direction))
                    validDirection.Add(new float3(0, 0, 1));
                if (!movementRay.CheckRay(transform.Value, new float3(-1, 0, 0), move.Direction))
                    validDirection.Add(new float3(-1, 0, 0));
                if (!movementRay.CheckRay(transform.Value, new float3(1, 0, 0), move.Direction))
                    validDirection.Add(new float3(1, 0, 0));
                if (validDirection.Length > 0)
                    move.Direction = validDirection[randomTemp.NextInt(validDirection.Length)];
                validDirection.Dispose();
            }
        }).Schedule();
        this.CompleteDependency();
    }

    private struct MovementRayCast
    {
        public PhysicsWorld PhysicsWorld;

        public bool CheckRay(float3 position, float3 direction, float3 currenDirection)
        {
            if (direction.Equals(-currenDirection))
            {
                return true;
            }
            var ray = new RaycastInput()
            {
                Start = position,
                End = position + (direction * 0.7f),
                Filter = new CollisionFilter()
                {
                    GroupIndex = 0,
                    BelongsTo = 1u << 1,
                    CollidesWith = 1u << 2,
                }
            };
            return PhysicsWorld.CastRay(ray);
        }
    }
}