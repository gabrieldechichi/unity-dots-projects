using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;


public partial class SpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var seed = (((uint) Time.ElapsedTime) + 1234) * 3;
        var rnd = new Random(seed);
        
        Entities
            .ForEach((Entity entity, in Spawner spawner, in DynamicBuffer<CarColor> carColors) =>
        {
            ecb.DestroyEntity(entity);
            for (var i = 0; i < spawner.LaneCount; i++)
            {
                var laneInstance = ecb.Instantiate(spawner.LanePrefab);
                var laneTranslation = new Translation
                {
                    Value = new float3(0, 0, i)
                };
                ecb.SetComponent(laneInstance, laneTranslation);

                for (var j = 0; j < 100; j++)
                {
                    if (rnd.NextInt() > spawner.CarFrequency)
                    {
                        var carInstance = ecb.Instantiate(spawner.CarPrefab);
                        ecb.SetComponent(carInstance, laneTranslation);

                        var carMovement = new CarMovement() {Offset = j};
                        ecb.SetComponent(carInstance, carMovement);

                        var color = carColors.Length > 0
                            ? (float3) carColors[rnd.NextInt(0, carColors.Length)]
                            : float3.zero;
                        
                        var urpColor = new URPMaterialPropertyBaseColor() {Value = new float4(color.xyz, 1)};
                        ecb.SetComponent(carInstance, urpColor);
                    }
                    
                }
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