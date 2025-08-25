using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState<T> : ICharacterState where T : ICharacterState<T> { }
public interface ICharacterState
{
    /// <summary>
    /// 메서드 호출 시점은 각 상태 객체가 생성된 이후 1회 호출.
    /// Unity 생명주기 -> Start
    /// </summary>
    void AwakeState() { }
    void OnStateEnter() { }
    void OnStateFixedUpdate() { }
    void OnStateUpdate() { }
    void OnStateExit() { }
    void OnTriggerEnter(Collider other) { }
    void OnTriggerStay(Collider other) { }
    void OnTriggerExit(Collider other) { }
}

public interface IStateController<TEnum>
    where TEnum : Enum
{
    void ChangeState(TEnum state);  
    ICharacterState GetState(TEnum stateName);
    TEnum GetCurrentStateType();
    ICharacterState GetCurrentState();
}

public interface ICharacterStateFactory<TClass, TEnum>
    where TClass : class
    where TEnum : Enum
{
    ICharacterState CreateState(TClass classType, TEnum enumType);
}

public interface IInteraction
{
    void Interact();
}

public interface IInteractionItem : IInteraction
{

}

public interface IInteractionDialog : IInteraction
{

}

public interface IInteractionGameObject : IInteraction
{
    bool IsWeaponInteractable { get; set; }
}

public interface ITakeDamage
{
    void TakeDamage(int damage);
}

public interface IUnbindAction
{
    void Unbind();
}

public interface IBurnable
{
    void Ignite();
    bool IsBurning();
}

public interface IInitializeEnable
{
    void Init();
}

public interface IStateBehaviourController
{
    void OnEnter(Animator animator, AnimatorStateInfo stateInfo) { }
    void OnUpdate(Animator animator, AnimatorStateInfo stateInfo) { }
    void OnExit(Animator animator, AnimatorStateInfo stateInfo) { }
}