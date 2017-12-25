using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Battle
{
    public class BattleTurn
    {
        
        public int Number { get; set; }

        public BaseAction PlayerBaseAction;

        public BaseAction OponentBaseAction;

        public BattleTurn(int number)
        {
            this.Number = number;
        }

        public CharacterType PriorityOwner
        {
            get
            {
                return (PlayerBaseAction.Priority >= OponentBaseAction.Priority) ? CharacterType.Player : CharacterType.Oponent;
            }
        }

        public List<ActionStep> ActionSteps { get; internal set; }

        public bool CheckIfPlayerTurnIsReady()
        {
            if (PlayerBaseAction == null)
                return false;

            //if (PlayerBaseAction.HasToMove && PlayerBaseAction.MoveTo == default(int))
            //    return false;
            
            return true;
            
        }

        public bool CheckTurnIsReady()
        {
            if (!CheckIfPlayerTurnIsReady()) 
                return false;
            
            if (OponentBaseAction == null)
                return false;

            return true;

        }
    }
}
