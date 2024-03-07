using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnerSystem : SystemBase
{
    private List<EnemyGroup> _enemyGroups;

    protected override void OnCreate()
    {
        AddAllEnemyGroup();
    }

    private void AddAllEnemyGroup()
    {
        _enemyGroups = new List<EnemyGroup>();
        
        var allEnemyGroup = Resources.LoadAll<EnemyGroup>("ScriptableObject/EnemyGroup");
        foreach (var grGroup in allEnemyGroup)
        {
            _enemyGroups.Add(grGroup);
        }
    }
    
    protected override void OnUpdate()
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

    private void CreateRandomEnemyGroup()
    { 
        // SpawnerComponent spawnerComponent = SystemAPI.GetSingleton<SpawnerComponent>();
        // var enemyInfos = spawnerComponent.EnemyInfosArray;
        // EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        // RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();
        // var randomGroup = randomComponent.ValueRW.Random.NextInt(0, enemyInfos.Length);
        //
        // var enemyInfo = enemyInfos[randomGroup];
        // CreateEnemy(enemyInfo, spawnerComponent);
    }

    // private void CreateEnemy(EnemyInfos enemyInfos, SpawnerComponent spawnerComponent)
    //  {
    //      EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
    //      RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();
    //      var randomPosition = GenerateRandomPosition(randomComponent, spawnerComponent);
    //      for (int i = 0; i < enemyInfos.count; i++)
    //      {
    //          Entity spawnEntity = entityCommandBuffer.Instantiate(enemyInfos.Entity);
    //          float3 offset = new float3( randomComponent.ValueRW.Random.NextFloat(-0.5f, 0.5f),  
    //                                         randomComponent.ValueRW.Random.NextFloat(-0.5f, 0.5f), 0f);
    //          float3 spawnPosition = randomPosition + offset;
    //           
    //          entityCommandBuffer.SetComponent(spawnEntity, new LocalTransform
    //          {
    //              Position = spawnPosition
    //          });
    //          entityCommandBuffer.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
    //      }
    //  }
    
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

