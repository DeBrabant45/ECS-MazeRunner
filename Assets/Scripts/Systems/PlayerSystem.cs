using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Entities
            .WithAll<Player>()
            .ForEach((ref Movable movable) => 
            {
                movable.Direction = new float3(horizontalInput, 0, verticalInput);
            }).Schedule();
    }
}