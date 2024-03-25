using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct MoveToPositionAspect : IAspect
{
    private readonly Entity _entity;
    
    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRO<SpeedComponent> _speed;
    private readonly RefRW<TargetPositionComponent> _target;

    public bool Move(float deltaTime, float reachedTargetDistance)
    {
        if (math.distancesq(_transform.ValueRW.Position, _target.ValueRW.value) > reachedTargetDistance)
        {
            var direction = math.normalize(_target.ValueRW.value - _transform.ValueRW.Position);
            
            _transform.ValueRW.Position += direction * deltaTime * _speed.ValueRO.value;
            _transform.ValueRW.Scale = direction.x > 0 ? -1 : 1;;
            return true;
        }

        return false;
    }

    public void TestReachedTargetPosition(RefRW<RandomComponent> randomComponent)
    {
        float reachedTargetDistance = .1f;
        if (math.distancesq(_transform.ValueRW.Position, _target.ValueRW.value) < reachedTargetDistance)
        {
            _target.ValueRW.value = GetRandomPosition(randomComponent);
        }
    }

    private float3 GetRandomPosition(RefRW<RandomComponent> randomComponent)
    {
        return new float3(
            randomComponent.ValueRW.Random.NextFloat(-17, 17),
            randomComponent.ValueRW.Random.NextFloat(-9, 9), 
            0
            );
    }
}
