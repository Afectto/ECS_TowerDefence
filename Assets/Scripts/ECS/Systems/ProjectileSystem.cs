using Unity.Entities;
using Unity.Transforms;

public partial struct ProjectileSystem : ISystem
{
    private ComponentLookup<LocalToWorld> position;
    
    public void OnUpdate(ref SystemState state)
    {
        
    }
}