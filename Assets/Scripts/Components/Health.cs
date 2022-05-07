using Unity.Entities;

[GenerateAuthoringComponent]
public struct Health : IComponentData
{
    public float Amount;
    public float InvincibleTimer;
    public float KillTimer;
}