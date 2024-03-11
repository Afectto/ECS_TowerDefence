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
        uint seed = math.hash((float2)UnityEngine.Random.Range(0, 100000));
        Debug.Log(seed);
        AddComponent(new RandomComponent
        {
            Random = new Random(seed)
        });
    }
}