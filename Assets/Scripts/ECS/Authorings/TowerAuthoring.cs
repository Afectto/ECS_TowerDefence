using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TowerAuthoring : MonoBehaviour
{
    public Transform FirePoint;
}

public class TowerBaker : Baker<TowerAuthoring>
{
    public override void Bake(TowerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var buffer = AddBuffer<WeaponInfoBufferElementData>(entity);
        
        AddComponent<TowerTag>(entity);
        AddComponent(entity, new TowerComponent
        {
            FirePoint = authoring.FirePoint.position
        });
        
    }
}