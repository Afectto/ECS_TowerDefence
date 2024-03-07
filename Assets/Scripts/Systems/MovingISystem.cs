using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

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
            targetDistanceComponents = SystemAPI.GetComponentLookup<AttackRangeComponent>(true),
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager
        }.Schedule(state.Dependency);
        
        jobHandle.Complete();
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float deltaTime;
    [ReadOnly]
    public ComponentLookup<AttackRangeComponent> targetDistanceComponents;
    public EntityManager entityManager;
    
    [BurstCompile]
    public void Execute(MoveToPositionAspect moveToPositionAspect, Entity entity)
    {
        float reachedTargetDistance = targetDistanceComponents[entity].distance;
        bool isMoving = moveToPositionAspect.Move(deltaTime, reachedTargetDistance);
        entityManager.SetComponentEnabled<WalkingStateTag>(entity, isMoving);
    }
}
