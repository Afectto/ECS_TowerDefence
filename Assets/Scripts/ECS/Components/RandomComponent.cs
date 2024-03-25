using Unity.Entities;
using Random = Unity.Mathematics.Random;

public struct RandomComponent : IComponentData
{
    public Random Random;
}
