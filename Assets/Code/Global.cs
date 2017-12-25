using Assets.Code.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    public class Global
    {

        public static Character PlayerData = new Character { Name = "Agnes", HP = MAX_HP, Type = CharacterType.Player, Position = PlayerStartPosition };

        public static int MAX_HP = 30;

        public static BattleManager BattleManager { get; internal set; }
        public static GameObject Camera;

        public static int MAX_X_POS = 8;
        public static int MIN_X_POS = 0;

        public static List<BaseAction> BaseActions = new List<BaseAction>
        {
             new BaseAction
             {
                  Name = "strike",
                  MinRange = 1,
                  MaxRange = 2,
                  Power = 4,
                  Priority = 3,
                  OnAfterActivation = delegate(BattleSystem battle)
                  {

                  }
             },
             new BaseAction
             {
                  Name = "dash",
                  MinRange = 1,
                  MaxRange = 3,
                  Power = 0,
                  Priority = 3,
                  HasToMove = true, 
                  OnAfterActivation = delegate(BattleSystem battle)
                  {

                  }
             }
        };
        public static readonly int PlayerStartPosition = 2;
        public static readonly int OponentStartPosition = 6;
        public static readonly int MAX_TURNS = 15;

        public static System.Random Random = new System.Random(DateTime.Now.Millisecond);
    }

    public static class GlobalExtensions {

        public static BaseAction GetByName(this List<BaseAction> actions, string name)
        {
            return (BaseAction)actions.Where(o => o.Name.ToLower() == name.ToLower()).First().Clone();
        }

        public static BaseAction GetRandomly(this List<BaseAction> actions)
        {
            int index = Global.Random.Next(0, Global.BaseActions.Count - 1);

            if (index > actions.Count - 1)
                index = actions.Count - 1;

            return (BaseAction)actions[index].Clone();

        }
    }
}
