using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityGameObject = UnityEngine.GameObject;
using UnityRangeAttribute = UnityEngine.RangeAttribute;
using UnityMonoBehaviour = UnityEngine.MonoBehaviour;
using UnityColor = UnityEngine.Color;

public class SpawnerAuthoring : UnityMonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public UnityGameObject LanePrefab;
    [UnityRange(0, 1000)] public int LaneCount;

    public UnityGameObject CarPrefab;
    [UnityRange(0, 1)] public float CarFrequency;

    public UnityColor[] CarColors;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        
        dstManager.AddComponentData(entity, new Spawner()
        {
            LanePrefab = conversionSystem.GetPrimaryEntity(LanePrefab),
            LaneCount = LaneCount,
            CarPrefab = conversionSystem.GetPrimaryEntity(CarPrefab),
            CarFrequency = CarFrequency,
        });
        
        dstManager.AddBuffer<CarColor>(entity);
        var buffer = dstManager.GetBuffer<CarColor>(entity);
        buffer.Capacity = CarColors.Length;
        foreach (var c in CarColors)
        {
            buffer.Add(new float3(c.r, c.g, c.b));
        }
    }

    public void DeclareReferencedPrefabs(List<UnityGameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(LanePrefab);
        referencedPrefabs.Add(CarPrefab);
    }
}