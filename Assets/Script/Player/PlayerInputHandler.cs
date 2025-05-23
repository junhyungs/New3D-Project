using PlayerComponent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    public PlayerInputHandler(Player player)
    {
        _input = player.GetComponent<PlayerInput>();
        BindAction();
    }

    private PlayerInput _input;
    private List<Action> _unbindActions = new List<Action>();

    public event Action RollEvent;
    public event Action RollSlashEvent;
    public event Action<Vector2> MoveEvent;
    public event Action InteractionEvent;

    private void BindAction()
    {
        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;

        _input.actions["Roll"].started += OnRoll;
        _input.actions["RollSlash"].started += OnRollSlash;
        _input.actions["Interaction"].started += OnInteraction;

        _unbindActions.Add(() => _input.actions["Move"].started -= OnMove);
        _unbindActions.Add(() => _input.actions["Move"].performed -= OnMove);
        _unbindActions.Add(() => _input.actions["Move"].canceled -= OnMove);

        _unbindActions.Add(() => _input.actions["Roll"].started -= OnRoll);
        _unbindActions.Add(() => _input.actions["RollSlash"].started -= OnRollSlash);
        _unbindActions.Add(() => _input.actions["Interaction"].started -= OnInteraction);
    }

    public void UnBindAction()
    {
        foreach (var action in _unbindActions)
            action.Invoke();

        _unbindActions.Clear();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        RollEvent.Invoke();
    }

    private void OnRollSlash(InputAction.CallbackContext context)
    {
        RollSlashEvent.Invoke();
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        InteractionEvent.Invoke();
    }
}
