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
            throw new NotImplementedException();
        }
    }
    #endregion
}

