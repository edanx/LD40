using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Util
{
    public class UIHelper
    {

        public static T Get<T>(string name)
        {
            return GameObject.Find(name).GetComponent<T>();
        }

        public static Transform GetGroundBlockByIndex(int selectedPosition)
        {
            return GameObject.Find("Quad_" + selectedPosition).transform;
        }

        internal static void HideAllBlocks()
        {
            for (int i = 0; i <= Global.MAX_X_POS; i++)
            {
                UIHelper.GetGroundBlockByIndex(i).GetComponent<Animator>().Play("block-idle");
            }
        }
    }
}
