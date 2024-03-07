using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<EnemyGroup> enemyGroups;
}

public class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new SpawnerComponent
        {
            xMin = -13f,
            xMax = 13f,
            yMin = -6f,
            yMax = 6f,
            avoidanceRadius = 20f,

            enemyPrefab = GetEntity(authoring.enemyPrefab),
            // entitiesBuffer = myEntitiesBuffer
        });
    }
}
