using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class CarMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var time = Time.ElapsedTime;
        Entities
            .WithAll<CarMovement>()
            .ForEach((ref Translation translation) =>
        {
            translation.Value.x = (float) time;
        }).ScheduleParallel();
    }
}
