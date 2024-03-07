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
        AddComponent(new AttackRangeComponent{
            distance = authoring.distance
        });
    }
}