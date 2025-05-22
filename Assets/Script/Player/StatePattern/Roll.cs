using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : PlayerMoveState, ICharacterState<Roll>
{
    public Roll(Player player) : base(player) { }
}
