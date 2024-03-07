using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class RandomAuthoring : MonoBehaviour
{
}

public class RandomBaker : Baker<RandomAuthoring>
{
    public override void Bake(RandomAuthoring authoring)
    {
        AddComponent(new RandomComponent
        {
            Random = new Random(1)
        });
    }
}