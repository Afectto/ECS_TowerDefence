using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemyPrefab;
    public int countInGroup;
}

public enum EnemyType
{
    Knight,
    Archer,
    Priest,
    Soldier,
    Thief
}
