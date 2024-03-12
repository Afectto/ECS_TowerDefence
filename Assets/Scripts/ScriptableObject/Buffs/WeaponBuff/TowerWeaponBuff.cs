using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "TowerWeaponBuff", menuName = "GameInfo/New TowerWeaponBuff")]
public class TowerWeaponBuff : Buffs<TypeWeapon, BuffWeapon>
{
}
