using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Util
{
    public class BattleHelper
    {
        public static bool CheckAttackRange(int ativePos,int reactivePos, int minRange, int maxRange) {
            
            //move to right
            if (ativePos < reactivePos)
            {
                return (reactivePos >= (ativePos + minRange) && reactivePos <= (ativePos + maxRange));
            }
            //move to left
            else
            {
                return (reactivePos >= (ativePos - maxRange) && reactivePos <= (ativePos - minRange));
            
            }
        }


    }
}
