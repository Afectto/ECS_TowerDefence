using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TowerBuff", menuName = "GameInfo/New TowerBuff")]
public class TowerBuff : Buffs<TypeTowerBuff, BuffTower>
{
}