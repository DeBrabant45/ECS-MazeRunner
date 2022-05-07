using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public partial class MovableSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, in Movable movable) => 
        {
            var step = movable.Direction * movable.Speed;
            physicsVelocity.Linear = step;
        }).Schedule();
    }
}