using Unity.Entities;
using UnityEngine;

public class WeaponAuthoring : MonoBehaviour
{
    public float Damage;
    public float AttackRite;
    public float AttackRange;
    public TypeWeapon TypeWeapon;
    public GameObject Bullet;
}

public class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new WeaponComponent
        {
            Damage = authoring.Damage,
            AttackRange = authoring.AttackRange,
            AttackRite = authoring.AttackRite,
            TypeWeapon = authoring.TypeWeapon,
            Bullet = GetEntity(authoring.Bullet)
        });
    }
}

