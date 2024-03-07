using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroup", menuName = "GameInfo/New EnemyGroup")]
public class EnemyGroup : ScriptableObject
{
    [SerializeField] private string _nameGroup;
    [SerializeField] private List<EnemyInfo> _enemyInfos;
    [SerializeField] private int _tier;

    public string NameGroup => _nameGroup;
    public List<EnemyInfo> EnemyInfos => _enemyInfos;
    public int Tier => _tier;
}