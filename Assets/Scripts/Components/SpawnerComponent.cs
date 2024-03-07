using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SpawnerComponent : IComponentData
{
    public Entity enemyPrefab;
    
    
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    
    public float avoidanceRadius;
}

