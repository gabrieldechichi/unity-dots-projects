using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//Tag component that marks an entity to be destroyed
public struct DestroyEntity : IComponentData
{
    
}

public struct CarMovement : IComponentData
{
    public float Offset;
}
