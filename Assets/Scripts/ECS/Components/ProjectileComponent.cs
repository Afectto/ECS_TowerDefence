﻿using Unity.Entities;

public struct ProjectileComponent : IComponentData
{
    public float Damage;
    public TypeWeapon TypeDamage;
}