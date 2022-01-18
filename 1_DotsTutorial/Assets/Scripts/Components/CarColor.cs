using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(32)]
public struct CarColor : IBufferElementData
{
    public static implicit operator float3(CarColor col)
    {
        return col.Value;
    }

    public static implicit operator CarColor(float3 f)
    {
        return new CarColor() {Value = f};
    }
    
    public float3 Value;
}