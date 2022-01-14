using System.Text.RegularExpressions;
using Unity.Entities;
using Unity.Rendering;

public partial class ColorPropagationSystem : SystemBase
{
    private EntityQuery propagateColorEntitiesQuery;
    private EntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = commandBufferSystem.CreateCommandBuffer();
        
        //schedule remove of propagate color for all entities that matches the query
        ecb.RemoveComponentForEntityQuery<PropagateColor>(propagateColorEntitiesQuery);

        var cdfe = GetComponentDataFromEntity<URPMaterialPropertyBaseColor>();

        Entities
            .WithNativeDisableContainerSafetyRestriction(cdfe)
            .WithStoreEntityQueryInField(ref propagateColorEntitiesQuery)
            .WithAll<PropagateColor>()
            .ForEach((in DynamicBuffer<LinkedEntityGroup> group, in URPMaterialPropertyBaseColor color) =>
            {
                for (var i = 0; i < group.Length; i++)
                {
                    //Random access URPMaterialPropertyBaseColor from child in group
                    //then set equal to parent's color
                    cdfe[group[i].Value] = color;
                }
            }).ScheduleParallel();
        
        //No need to playback or dispoce ecb as it will be responsability of the EndSimulationSystem
    }
}