using Unity.Entities;

public struct WalkingStateTag : IComponentData, IEnableableComponent
{
    public bool active;
}
