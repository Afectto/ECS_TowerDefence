using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public partial struct MovingISystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();
        float deltaTime = SystemAPI.Time.DeltaTime;

        JobHandle jobHandle = new MoveJob
        {
            deltaTime = deltaTime,
            targetDistanceComponents = SystemAPI.GetComponentLookup<AttackRangeComponent>(true)
        }.ScheduleParallel(state.Dependency);
        
        jobHandle.Complete();
        //
        // new TestReachedTargetPositionJob
        // {
        //     randomComponent = randomComponent
        // }.Run();
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float deltaTime;
    [ReadOnly]
    public ComponentLookup<AttackRangeComponent> targetDistanceComponents;
    
    [BurstCompile]
    public void Execute(ref MoveToPositionAspect moveToPositionAspect)
    {
        //float reachedTargetDistance = targetDistanceComponents[].distance;
        moveToPositionAspect.Move(deltaTime,10);
    }
}

[BurstCompile]
public partial struct TestReachedTargetPositionJob : IJobEntity
{
    [NativeDisableUnsafePtrRestriction]public RefRW<RandomComponent> randomComponent;
    
    [BurstCompile]
    public void Execute(MoveToPositionAspect moveToPositionAspect)
    {
        moveToPositionAspect.TestReachedTargetPosition(randomComponent);
    }
}