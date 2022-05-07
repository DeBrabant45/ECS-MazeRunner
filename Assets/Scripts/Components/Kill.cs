using Unity.Entities;

[GenerateAuthoringComponent]
public struct Kill : IComponentData
{
    public float Timer;
}