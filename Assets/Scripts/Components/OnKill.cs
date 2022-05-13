using Unity.Entities;
using Unity.Collections;

public struct OnKill : IComponentData
{
    public FixedString64Bytes SFXName;
    public Entity SpawnPrefab;
    public int Point;
}