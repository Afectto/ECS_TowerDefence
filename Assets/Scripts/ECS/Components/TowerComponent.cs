using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct TowerComponent : IComponentData
{
    public float3 FirePoint;
    // public DynamicBuffer<WeaponInfo> WeaponList;
}