using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState<T> : ICharacterState where T : ICharacterState<T> { }
public interface ICharacterState
{
    void OnStateEnter() { }
    void OnStateFixedUpdate() { }
    void OnStateUpdate() { }
    void OnStateExit() { }
    void OnTriggerEnter(Collider other) { }
    void OnTriggerStay(Collider other) { }
    void OnTriggerExit(Collider other) { }
}

public interface IGetState<T> where T : Enum
{
    ICharacterState GetCurrentState();
    ICharacterState GetState(T stateName);
    T GetCurrentStateType();
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

}

public interface ITakeDamage
{
    void TakeDamage(float damage);
}

public interface IUnbindAction
{
    void Unbind();
}
