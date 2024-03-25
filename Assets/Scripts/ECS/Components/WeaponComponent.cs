using Unity.Entities;

public struct WeaponComponent : IComponentData
{
    public float Damage;
    public float AttackRite;
    public float AttackRange;
    public TypeWeapon TypeWeapon;
    public Entity Bullet;
}
