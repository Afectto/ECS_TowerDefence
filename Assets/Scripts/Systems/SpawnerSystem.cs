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


        int spawnAmount = 1;
        if (enemyEntityQuery.CalculateEntityCount() < spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                CreateEnemy();
            }
        }
    }

    private void CreateEnemy()
    {
        SpawnerComponent spawnerComponent = SystemAPI.GetSingleton<SpawnerComponent>();

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();

        Entity spawnEntity = entityCommandBuffer.Instantiate(spawnerComponent.enemyPrefab);

        entityCommandBuffer.SetComponent(spawnEntity, new LocalTransform
        {
            Position = GenerateRandomPosition(randomComponent, spawnerComponent)
        });
        entityCommandBuffer.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
        
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

