using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Util
{
    public class Move
    {

        public static int NextPositionForward(int actualPosition, int oponentPosition, int minRange, int maxRange)
        {
            //move to right
            if (actualPosition < oponentPosition)
            {
                var pos = Math.Min(actualPosition + minRange, actualPosition + maxRange);

                if (oponentPosition == pos)
                {
                    pos++;
                }
                return (pos <= Global.MAX_X_POS) ? pos : actualPosition;
            }
            //move to left
            else
            {
                var pos = Math.Max(actualPosition - maxRange, actualPosition - minRange);

                if (oponentPosition == pos)
                {
                    pos--;
                }
                return (pos >= Global.MIN_X_POS) ? pos : actualPosition;
            }

        }

        public static int[] MovablePositions(int actualPosition, int oponentPosition, int minRange, int maxRange)
        {

            var result = new List<int>();

            //move to right
            //if (actualPosition < oponentPosition)
            //{

                for (int i = actualPosition + minRange; i <= actualPosition + maxRange; i++)
                {
                    if (i != oponentPosition && i <= Global.MAX_X_POS)
                        result.Add(i);
                }

            //}
            //move to left
            //else
            //{
                for (int i = actualPosition - maxRange; i <= actualPosition - minRange; i++)
                {
                    if (i != oponentPosition && i >= Global.MIN_X_POS)
                        result.Add(i);
                }
            ///}

            return result.ToArray();

        }

        public static int[] AtackPositions(int actualPosition, int oponentPosition, int minRange, int maxRange)
        {

            var result = new List<int>();

            //move to right
            if (actualPosition < oponentPosition)
            {

                for (int i = actualPosition + minRange; i <= actualPosition + maxRange; i++)
                {
                    result.Add(i);
                }

            }
            //move to left
            else
            {
                for (int i = actualPosition - maxRange; i <= actualPosition - minRange; i++)
                {
                    result.Add(i);
                }
            }

            return result.ToArray();

        }
    }
}
