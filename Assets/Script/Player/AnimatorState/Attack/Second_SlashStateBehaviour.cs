using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Second_SlashStateBehaviour : AttackStateBehaviour
{
    private void Awake()
    {
        _hand = PlayerHand.Left;
    }

}
