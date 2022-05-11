using Unity.Entities;

[GenerateAuthoringComponent]
public struct Collectable : IComponentData
{
    public int Points;
}