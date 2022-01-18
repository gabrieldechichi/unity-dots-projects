using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

public partial class PlayerCarCollisionSystem : SystemBase
{
    private EntityCommandBufferSystem commandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }
    
    protected override void OnUpdate()
    {
        static bool IsOverlapping(in WorldRenderBounds a, in WorldRenderBounds b)
        {
            return
                math.abs(a.Value.Center.x - b.Value.Center.x) < (a.Value.Extents.x + b.Value.Extents.x) &&
                math.abs(a.Value.Center.y - b.Value.Center.y) < (a.Value.Extents.y + b.Value.Extents.y) &&
                math.abs(a.Value.Center.z - b.Value.Center.z) < (a.Value.Extents.z + b.Value.Extents.z);
        }
        
        var player = GetSingletonEntity<Player>();
        var playerWorldBounds = GetComponent<WorldRenderBounds>(player);

        var ecb = commandBufferSystem.CreateCommandBuffer();

        Entities
            .WithAll<CarMovement>()
            .ForEach((Entity carEntity, in WorldRenderBounds carBounds) =>
        {
            if (IsOverlapping(carBounds, playerWorldBounds))
            {
                ecb.DestroyEntity(carEntity);
            }
        }).Run();
    }
}