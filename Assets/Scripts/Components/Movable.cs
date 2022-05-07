using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Movable : IComponentData
{
    public float Speed;
    public float3 Direction;
}