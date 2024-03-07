using Unity.Collections;
using Unity.Entities;

public struct SpawnerComponent : IComponentData
{
    public Entity enemyPrefab;
    
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    
    public float avoidanceRadius;

}

