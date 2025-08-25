using PlayerComponent;
using EnemyComponent;
using System;
using System.Collections.Generic;
using EnumCollection;

namespace State
{
    public class CharacterStateFactory<TClass, TEnum, TStateFactory>
       where TClass : class
       where TEnum : Enum
       where TStateFactory : ICharacterStateFactory<TClass, TEnum>, new()
    {
        private TStateFactory _factory;

        public CharacterStateFactory()
        {
            _factory = new TStateFactory();
        }

        public Dictionary<TEnum, ICharacterState> CreateState(TClass classType)
        {
            Dictionary<TEnum, ICharacterState> stateDictionary = new Dictionary<TEnum, ICharacterState>();

            var enumArray = Enum.GetValues(typeof(TEnum));
            foreach (TEnum enumTpye in enumArray)
            {
                ICharacterState newState = _factory.CreateState(classType, enumTpye);
                stateDictionary.Add(enumTpye, newState);
            }

            return stateDictionary;
        }
    }

    #region Player
    public class PlayerStateFactory : ICharacterStateFactory<Player, E_PlayerState>
    {
        public ICharacterState CreateState(Player classType, E_PlayerState enumType)
        {
            switch (enumType)
            {
                case E_PlayerState.Idle:
                    return new Idle(classType);
                case E_PlayerState.Move:
                    return new Move(classType);
                case E_PlayerState.Roll:
                    return new Roll(classType);
                case E_PlayerState.Falling:
                    return new Falling(classType);
                case E_PlayerState.RollSlash:
                    return new RollSlash(classType);
                case E_PlayerState.Climbing:
                    return new Climbing(classType);
                case E_PlayerState.Attack:
                    return new Attack(classType);
                case E_PlayerState.ChargeAttack:
                    return new ChargeAttack(classType);
                case E_PlayerState.Skill:
                    return new Skill(classType);
                default:
                    throw new ArgumentException();
            }
        }
    }
    #endregion
    #region Mage
    public class MageStateFactory : ICharacterStateFactory<Mage, E_MageState>
    {
        public ICharacterState CreateState(Mage classType, E_MageState enumType)
        {
            switch (enumType)
            {
                case E_MageState.Idle:
                    return new MageIdle(classType);
                case E_MageState.Move:
                    return new MageMove(classType);
                case E_MageState.Teleport:
                    return new MageTeleport(classType);
                case E_MageState.Attack:
                    return new MageAttack(classType);
                case E_MageState.Death:
                    return new MageDeath(classType);
                default
                    : throw new ArgumentException();
            }
        }
    }
    #endregion
    #region Ghoul
    public class GhoulStateFactory : ICharacterStateFactory<Ghoul, E_GhoulState>
    {
        public ICharacterState CreateState(Ghoul classType, E_GhoulState enumType)
        {
            switch (enumType)
            {
                case E_GhoulState.Idle:
                    return new GhoulIdle(classType);    
                case E_GhoulState.Partrol:
                    return new GhoulPatrol(classType);
                case E_GhoulState.Trace:
                    return new GhoulTrace(classType);
                case E_GhoulState.Attack:
                    return new GhoulAttack(classType);
                case E_GhoulState.Death:
                    return new GhoulDeath(classType);
                case E_GhoulState.CoolDown:
                    return new GhoulCoolDown(classType);
                default: 
                    throw new ArgumentException();
            }
        }
    }
    #endregion
    #region Bat
    public class BatStateFactory : ICharacterStateFactory<Bat, E_BatState>
    {
        public ICharacterState CreateState(Bat classType, E_BatState enumType)
        {
            switch (enumType)
            {
                case E_BatState.Patrol:
                    return new BatPatrol(classType);
                case E_BatState.Trace:
                    return new BatTrace(classType);
                case E_BatState.Attack:
                    return new BatAttack(classType);
                case E_BatState.Death:
                    return new BatDeath(classType);
                default:
                    throw new ArgumentException();
            }
        }
    }
    #endregion
    #region Slime
    public class SlimeFactory : ICharacterStateFactory<Slime, E_SlimeState>
    {
        public ICharacterState CreateState(Slime classType, E_SlimeState enumType)
        {
            switch (enumType)
            {
                case E_SlimeState.Patrol:
                    return new SlimePatrol(classType);
                case E_SlimeState.Trace:
                    return new SlimeTrace(classType);
                case E_SlimeState.Attack:
                    return new SlimeAttack(classType);
                case E_SlimeState.Death:
                    return new SlimeDeath(classType);
                case E_SlimeState.Idle:
                    return new SlimeIdle(classType);
                case E_SlimeState.CoolDown:
                    return new SlimeCoolDown(classType);
                default: 
                    throw new ArgumentException();
            }
        }
    }
    #endregion
}

