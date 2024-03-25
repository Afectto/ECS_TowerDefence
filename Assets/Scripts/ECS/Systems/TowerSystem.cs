using Unity.Entities;

public partial class TowerSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<TowerComponent>();
    }

    protected override void OnUpdate()
    {
        var towerComponent = SystemAPI.GetSingleton<TowerComponent>();
        var towerEntity = SystemAPI.GetSingletonEntity<TowerComponent>();
        var firePoint = towerComponent.FirePoint;
        var weaponBuffer = SystemAPI.GetBuffer<WeaponInfoBufferElementData>(towerEntity);

    }
}