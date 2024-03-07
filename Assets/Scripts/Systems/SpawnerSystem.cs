using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnerSystem : ISystem
{
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityQuery enemyEntityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(EnemyTag));

        SpawnerComponent spawnerComponent = SystemAPI.GetSingleton<SpawnerComponent>();

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();
        
        int spawnAmount = 1000;
        if (enemyEntityQuery.CalculateEntityCount() < spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                CreateEnemy(entityCommandBuffer, spawnerComponent, randomComponent);
            }
        }
        entityCommandBuffer.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
    }

    private void CreateEnemy(EntityCommandBuffer entityCommandBuffer, SpawnerComponent spawnerComponent, RefRW<RandomComponent> randomComponent)
    {
        Entity spawnEntity = entityCommandBuffer.Instantiate(spawnerComponent.enemyPrefab);
                
        entityCommandBuffer.SetComponent(spawnEntity, new SpeedComponent
        {
            value = randomComponent.ValueRW.Random.NextFloat(1f, 5f)
        });
        
        entityCommandBuffer.SetComponent(spawnEntity, new LocalTransform
        {
            Position = GenerateRandomPosition(randomComponent, spawnerComponent)
        });
        
    }
    private float3 GenerateRandomPosition(RefRW<RandomComponent> randomComponent,SpawnerComponent spawnerComponent)
    {
        float randX, randY;
        do
        {
            randX = randomComponent.ValueRW.Random.NextFloat(spawnerComponent.xMin, spawnerComponent.xMax);
            randY = randomComponent.ValueRW.Random.NextFloat(spawnerComponent.yMin, spawnerComponent.yMax);
        } while (math.distancesq(new float2(randX, randY), float2.zero) < spawnerComponent.avoidanceRadius);

        return new float3(randX, randY, 0);
    }
}

