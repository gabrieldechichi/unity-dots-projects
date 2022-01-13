using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        
        Entities
            .ForEach((Entity entity, in Spawner spawner) =>
        {
            ecb.DestroyEntity(entity);
            for (int i = 0; i < spawner.LaneCount; i++)
            {
                var instance = ecb.Instantiate(spawner.LanePrefab);
                var translation = new Translation
                {
                    Value = new float3(0, 0, i)
                };
                ecb.SetComponent(instance, translation);
            }
        })
            //Using Run here forces the job to run on the main thread. And no job scheduling takes place
            //This has pros and cons.
                //Pros: Avoid job scheduling overhead for simple operations
                // Cons: No multithreading
                //Does the sync point happens anyway?
            .Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}