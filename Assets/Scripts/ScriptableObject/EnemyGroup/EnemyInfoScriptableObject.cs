using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyGroup", menuName = "GameInfo/New EnemyInfo")]
public class EnemyInfoScriptableObject : ScriptableObject 
{
    public GameObject enemyPrefab;
    public int countInGroup;
}
