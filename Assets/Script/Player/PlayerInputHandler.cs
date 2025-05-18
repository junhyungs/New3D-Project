using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    public PlayerInputHandler(PlayerInput input)
    {
        _input = input;

        BindAction();
    }

    private PlayerInput _input;

    public Vector2 MoveVector {  get; private set; }

    private void BindAction()
    {
        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();
    }
}
