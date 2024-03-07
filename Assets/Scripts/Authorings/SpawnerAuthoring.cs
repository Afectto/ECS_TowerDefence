using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
}

public class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        AddComponent(new SpawnerComponent
        {
            xMin = -13f,
            xMax = 13f,
            yMin = -6f,
            yMax = 6f,
            avoidanceRadius = 7f,
            enemyPrefab = GetEntity(authoring.enemyPrefab)
        });
    }
}
