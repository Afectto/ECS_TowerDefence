using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PresentationGameObject : IComponentData
{
    public GameObject prefab;
}

public class AnimatorGameObject : ICleanupComponentData
{
    public Animator value;
}

public class AnimatorAuthoring : MonoBehaviour
{
    public GameObject GameObjectPrefab;
    
    public class GameObjectPrefabBaker : Baker<AnimatorAuthoring>
    {
        public override void Bake(AnimatorAuthoring authoring)
        {
            var presentationGameObject = new PresentationGameObject {prefab = authoring.GameObjectPrefab};
            AddComponentObject(presentationGameObject);
        }
    }
}
