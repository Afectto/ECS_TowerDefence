using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemyPrefab;
    public int countInGroup;
}

public enum EnemyType
{
    Archer,
    Knight,
    Merchant,
    Priest,
    Soldier,
    Thief
}
