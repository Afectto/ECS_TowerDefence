using Unity.Entities;
using UnityEngine;

public class AttackRangeAuthoring : MonoBehaviour
{
    public float distance;
}

public class AttackRangeBaker : Baker<AttackRangeAuthoring>
{
    public override void Bake(AttackRangeAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new AttackRangeComponent{
            distance = authoring.distance
        });
    }
}