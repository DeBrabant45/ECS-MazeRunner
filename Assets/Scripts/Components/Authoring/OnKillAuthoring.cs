using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class OnKillAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [SerializeField] private string _sfxName;
    [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private int _point;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity,
            new OnKill
            {
                SFXName = new FixedString64Bytes(_sfxName),
                Point = _point,
                SpawnPrefab = conversionSystem.GetPrimaryEntity(_spawnPrefab)
            }); 
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(_spawnPrefab);
    }
}