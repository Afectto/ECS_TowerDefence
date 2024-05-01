using Unity.Collections;
using Unity.Entities;
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
        UpdateAttackAnimation(ref state);
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
        foreach (var (transform, animatorReference, entity) in 
            SystemAPI.Query<LocalTransform, AnimatorGameObject>().WithEntityAccess())
        {     
            var walkingState = entityManager.IsComponentEnabled<WalkingStateTag>(entity);
            ChangeAnimationIfNeed("IsWalking", animatorReference, walkingState);
            
            animatorReference.value.transform.position = transform.Position;
            animatorReference.value.transform.rotation = transform.Rotation;
            animatorReference.value.transform.localScale = new Vector3(transform.Scale, 1, 1);
        }
    }

    private void UpdateAttackAnimation(ref SystemState state)
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (var (animatorReference, entity) in 
            SystemAPI.Query<AnimatorGameObject>().WithEntityAccess())
        {     
            var attackingState = entityManager.IsComponentEnabled<AttackingStateTag>(entity);
            ChangeAnimationIfNeed("IsAttaking", animatorReference, attackingState);
        }
    }

    private void ChangeAnimationIfNeed(string nameAnimation, AnimatorGameObject animatorReference, bool state)
    {
        var currentState = animatorReference.value.GetBool(nameAnimation);
        if (currentState != state)
        {
            animatorReference.value.SetBool(nameAnimation, state);
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
