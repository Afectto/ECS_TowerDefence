using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyTagAuthoring : MonoBehaviour
{
}

public class EnemyTagBaker : Baker<EnemyTagAuthoring>
{
    public override void Bake(EnemyTagAuthoring authoring)
    {
        AddComponent(new EnemyTag());
        AddComponent(new AttackingStateTag());
        SetComponentEnabled<AttackingStateTag>(false);
        
        AddComponent(new DieStateTag());
        SetComponentEnabled<DieStateTag>(false);
        
        AddComponent(new HurtStateTag());
        SetComponentEnabled<HurtStateTag>(false);
        
        AddComponent(new WalkingStateTag());
        SetComponentEnabled<WalkingStateTag>(false);
    }
}