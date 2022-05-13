using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new CameraTag() { });
    }
}