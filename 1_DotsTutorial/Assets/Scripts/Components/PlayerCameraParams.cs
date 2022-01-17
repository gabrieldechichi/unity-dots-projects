using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerCameraParams : IComponentData
{
    public float3 CameraOffset;
    public float SmoothTime;
    public float MaxSpeed;
}