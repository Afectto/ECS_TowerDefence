using System.Collections.Generic;
using Unity.Entities;
    using UnityEngine;

    public struct EnemyInfoBuffer : IBufferElementData
    {
        public Entity enemyPrefab;
        public int count;
    }
    
    
    public class SpawnerAuthoring : MonoBehaviour
    {
        public List<EnemyInfoScriptableObject> enemyInfoList;
    }
    
    public class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            var buffer = AddBuffer<EnemyInfoBuffer>(entity);
            for (int i = 0; i < authoring.enemyInfoList.Count; i++)
            {
                var infos = authoring.enemyInfoList[i];
                buffer.Add(new EnemyInfoBuffer
                {
                    enemyPrefab = GetEntity(infos.enemyPrefab),
                    count = infos.countInGroup
                });
            }
            
            AddComponent(entity, new SpawnerComponent
            {
                xMin = -13f,
                xMax = 13f,
                yMin = -6f,
                yMax = 6f,
                avoidanceRadius = 20f,

            });
        }

    }