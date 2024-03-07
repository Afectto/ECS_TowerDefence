using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct AnimateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        CreateAnimationGameObject(ref state);
        UpdateAnimationPosition(ref state);
        UpdateRemoveComponentOnDestroy(ref state);
    }

    private void CreateAnimationGameObject(ref SystemState state)
    {
        
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var (pgo,entity) in SystemAPI.Query<PresentationGameObject>().WithEntityAccess())
        {
            GameObject go = GameObject.Instantiate(pgo.prefab);
            go.AddComponent<EntityGameObject>().AssignEntity(entity, state.World);

            ecbBOS.AddComponent(entity, new AnimatorGameObject() { value = go.GetComponent<Animator>() });

            ecbBOS.RemoveComponent<PresentationGameObject>(entity);
        }

    }

    private void UpdateAnimationPosition(ref SystemState state)
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (var (transform, animatorReference, target, entity) in 
            SystemAPI.Query<LocalTransform, AnimatorGameObject, TargetPositionComponent>().WithEntityAccess())
        {     
            var walkingState = entityManager.IsComponentEnabled<WalkingStateTag>(entity);
            animatorReference.value.SetBool("IsWalking", walkingState);
            animatorReference.value.transform.position = transform.Position;
            animatorReference.value.transform.rotation = transform.Rotation;
            animatorReference.value.transform.localScale = new Vector3(transform.Scale, 1, 1);
        }
    }

    private void UpdateRemoveComponentOnDestroy(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
            

        foreach (var (animatorReference, entity) in 
            SystemAPI.Query<AnimatorGameObject>().WithNone<PresentationGameObject, LocalTransform>()
                .WithEntityAccess())
        {
            Object.Destroy(animatorReference.value.gameObject);
            ecb.RemoveComponent<PresentationGameObject>(entity);
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
