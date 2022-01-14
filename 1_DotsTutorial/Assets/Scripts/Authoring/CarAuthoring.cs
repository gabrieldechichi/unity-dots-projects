using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityMonoBehaviour = UnityEngine.MonoBehaviour;
using UnityMeshRenderer = UnityEngine.MeshRenderer;

public class CarAuthoring : UnityMonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var allRenderers = GetComponentsInChildren<UnityMeshRenderer>();
        var needPropagateColor = new NativeArray<Entity>(allRenderers.Length, Allocator.Temp);

        for (var i = 0; i < allRenderers.Length; i++)
        {
            var meshRenderer = allRenderers[i];
            needPropagateColor[i] = conversionSystem.GetPrimaryEntity(meshRenderer.gameObject);
        }

        dstManager.AddComponent<URPMaterialPropertyBaseColor>(needPropagateColor);
        
        dstManager.AddComponent<PropagateColor>(entity);
        dstManager.AddComponent<CarMovement>(entity);
    }
}
