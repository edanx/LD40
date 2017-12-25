using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Battle
{

    public enum CharacterType
    {
        Player = 0,
        Oponent = 1
    }

    public class Character
    {
        public string Name { get; set; }

        public int HP { get; set; }

        public int Position { get; set; }

        public bool Stunned { get; set; }

        public CharacterType Type { get; set; }
    }
}
