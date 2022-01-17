using Unity.Entities;
using UnityEngineCamera = UnityEngine.Camera;  

[GenerateAuthoringComponent]
public class GameObjectRefs : IComponentData
{
    public UnityEngineCamera Camera;
}