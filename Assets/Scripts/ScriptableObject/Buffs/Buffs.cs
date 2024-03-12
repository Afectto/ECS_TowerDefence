using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public abstract class Buffs<TType,TBuff> : ScriptableObject
{
    [SerializeField] private TType Type;
    [SerializeField] private List<TBuff> ListBuffs;

    public  TBuff GetRandomBuff()
    {
        var rBuffIndex = Random.Range(0, ListBuffs.Count);
        return ListBuffs[rBuffIndex];
    }
}