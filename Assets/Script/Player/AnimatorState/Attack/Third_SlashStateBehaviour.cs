using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Third_SlashStateBehaviour : AttackStateBehaviour
{
    private void Awake()
    {
        _hand = PlayerHand.Right;
    }
}
