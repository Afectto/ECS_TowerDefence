using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnerSystem : SystemBase
{
    private List<EnemyGroup> _enemyGroups;
    private float timer = Single.MaxValue;
    
    protected override void OnUpdate()
    {
        timer += SystemAPI.Time.DeltaTime;

        if (timer >= 0.25f)
        {
            CreateRandomEnemyGroup();
            timer = 0;
        }
    }

    private void CreateRandomEnemyGroup()
    {
        var spawnerComponent = SystemAPI.GetSingleton<SpawnerComponent>();
        var spawnerComponentEntity = SystemAPI.GetSingletonEntity<SpawnerComponent>();
        
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();
        
        var buffer = GetBufferLookup<EnemyInfoBuffer>()[spawnerComponentEntity];
        var randomGroup = randomComponent.ValueRW.Random.NextFloat(0, buffer.Length);
        var group = buffer[(int)randomGroup];
        var randomPosition = GenerateRandomPosition(randomComponent, spawnerComponent);

        for (int i = 0; i < group.count; i++)
        {
            Entity spawnEntity = entityCommandBuffer.Instantiate(group.enemyPrefab);
        
            var offset = new float3(randomComponent.ValueRW.Random.NextFloat(-0.5f, 0.5f), randomComponent.ValueRW.Random.NextFloat(0, buffer.Length), 0);
            entityCommandBuffer.SetComponent(spawnEntity, new LocalTransform
            {
                Position = randomPosition + offset
            });
        }

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

