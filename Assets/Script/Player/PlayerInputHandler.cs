using PlayerComponent;
using System;
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
    public event Action RollEvent;
    public event Action RollSlashEvent;
    public event Action<Vector2> MoveEvent;

    private void BindAction()
    {
        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;

        _input.actions["Roll"].started += OnRoll;
        _input.actions["RollSlash"].started += OnRollSlash;
        
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
}
