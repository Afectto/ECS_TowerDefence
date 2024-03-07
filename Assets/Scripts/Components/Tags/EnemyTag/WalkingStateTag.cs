using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct WalkingStateTag : IComponentData, IEnableableComponent
{
    public bool active;
}
