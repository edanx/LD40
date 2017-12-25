using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Battle
{
    public class BaseAction : ICloneable
    {

        public delegate void ActionScript(BattleSystem battle);

        public string Name { get; set; }

        public int Power { get; set; }

        public int MinRange { get; set; }

        public int MaxRange { get; set; }

        public int Priority { get; set; }

        public bool HasToMove { get; set; }

        public int Soak { get; set; }

        public int MoveTo { get; set; }

        public ActionScript OnBeforeActivation;

        public ActionScript OnHitTheOponent;

        public ActionScript OnAfterActivation;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
