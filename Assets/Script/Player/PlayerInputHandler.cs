using EnumCollection;
using PlayerComponent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : IUnbindAction
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
    public event Action SlashEvent;
    public event Action<bool> ChargeSlashEvent;
    public event Action<bool> SkillEvent;
    public event Action<string> ChangeSkillEvent;

    private void BindAction()
    {
        _input.actions["Move"].started += OnMove;
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;

        _input.actions["Roll"].started += OnRoll;
        _input.actions["RollSlash"].started += OnRollSlash;

        _input.actions["Interaction"].started += OnInteraction;

        _input.actions["Slash"].started += OnSlash;
        _input.actions["ChargeSlash"].performed += OnChargeSlash;
        _input.actions["ChargeSlash"].canceled += OnChargeSlash;

        _input.actions["Skill"].started += OnSkill;
        _input.actions["Skill"].canceled += OnSkill;
        _input.actions["ChangeSkill"].started += OnChangeSkill;

        _unbindActions.Add(() => _input.actions["Move"].started -= OnMove);
        _unbindActions.Add(() => _input.actions["Move"].performed -= OnMove);
        _unbindActions.Add(() => _input.actions["Move"].canceled -= OnMove);

        _unbindActions.Add(() => _input.actions["Roll"].started -= OnRoll);
        _unbindActions.Add(() => _input.actions["RollSlash"].started -= OnRollSlash);
        _unbindActions.Add(() => _input.actions["Interaction"].started -= OnInteraction);
        _unbindActions.Add(() => _input.actions["Slash"].started -= OnSlash);

        _unbindActions.Add(() => _input.actions["ChargeSlash"].performed -= OnChargeSlash);
        _unbindActions.Add(() => _input.actions["ChargeSlash"].canceled -= OnChargeSlash);

        _unbindActions.Add(() => _input.actions["Skill"].started -= OnSkill);
        _unbindActions.Add(() => _input.actions["Skill"].canceled -= OnSkill);
        _unbindActions.Add(() => _input.actions["ChangeSkill"].started -= OnChangeSkill);
    }

    public void Unbind()
    {
        foreach (var action in _unbindActions)
            action.Invoke();

        _unbindActions.Clear();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        RollEvent?.Invoke();
    }

    private void OnRollSlash(InputAction.CallbackContext context)
    {
        RollSlashEvent?.Invoke();
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        InteractionEvent?.Invoke();
    }

    private void OnSlash(InputAction.CallbackContext context)
    {
        SlashEvent?.Invoke();
    }

    private void OnChargeSlash(InputAction.CallbackContext context)
    {
        ChargeSlashEvent?.Invoke(context.ReadValueAsButton());
    }

    private void OnSkill(InputAction.CallbackContext context)
    {
        SkillEvent?.Invoke(context.ReadValueAsButton());
    }

    private void OnChangeSkill(InputAction.CallbackContext context)
    {
        ChangeSkillEvent?.Invoke(context.control.name);
    }

}
