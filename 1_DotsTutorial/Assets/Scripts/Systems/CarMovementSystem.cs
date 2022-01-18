using Unity.Entities;
using Unity.Transforms;

public partial class CarMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var time = Time.ElapsedTime;
        Entities
            .ForEach((ref Translation translation, in CarMovement carMovement) =>
        {
            translation.Value.x = (float) ((time + carMovement.Offset) % 100) - 50.0f;
        }).ScheduleParallel();
    }
}
