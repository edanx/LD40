using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Code.Util;
using System.Linq;

public class UIActionSelection : MonoBehaviour
{

    Text txt_action_selected;
    Text txt_battle_info;
    Text txt_selected_action_info;

    Slider player_hp;
    Slider enemy_hp;

    GameObject ui_line_move;

    // Use this for initialization
    void Start()
    {
        txt_action_selected = GameObject.Find("txt_selected_action").GetComponent<Text>();
        txt_battle_info = GameObject.Find("txt_battle_info").GetComponent<Text>();
        txt_selected_action_info = GameObject.Find("txt_selected_action_info").GetComponent<Text>();
        ui_line_move = GameObject.Find("ui_line_move");

        player_hp = GameObject.Find("player_hp").GetComponent<Slider>();
        enemy_hp = GameObject.Find("enemy_hp").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        player_hp.value = Global.BattleManager.Battle.Player.HP;
        enemy_hp.value = Global.BattleManager.Battle.Enemy.HP;

        var actionSelected = Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction;

        txt_action_selected.text = (actionSelected == null) ? "select an action!" : "< " + actionSelected.Name + " >";

        txt_battle_info.text = string.Format("turn: {0}", Global.BattleManager.Battle.CurrentTurn.Number.ToString());


        var movimentSelection = (actionSelected != null && actionSelected.HasToMove);

        if (actionSelected != null)
        {

            if (actionSelected.HasToMove)
            {

                ui_line_move.GetComponent<UILineParabola>().PositionA = Global.BattleManager.Player;


                var moveBlock = UIHelper.GetGroundBlockByIndex(Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MoveTo);
                var positions = Move.MovablePositions(Global.BattleManager.Battle.Player.Position, Global.BattleManager.Battle.Enemy.Position, actionSelected.MinRange, actionSelected.MaxRange);

                moveBlock.GetComponent<Animator>().Play("block-move-to");


                foreach (var pos in positions)
                {
                    if (pos != Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MoveTo)
                        UIHelper.GetGroundBlockByIndex(pos).GetComponent<Animator>().Play("block-move-to-unselected");
                }

                ui_line_move.GetComponent<UILineParabola>().PositionB = moveBlock;

                HideOthersBlocks(Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MoveTo, positions);
            }
            else
            {
                var positions = Move.AtackPositions(Global.BattleManager.Battle.Player.Position, Global.BattleManager.Battle.Enemy.Position, actionSelected.MinRange, actionSelected.MaxRange);

                foreach (var pos in positions)
                {
                    UIHelper.GetGroundBlockByIndex(pos).GetComponent<Animator>().Play("block-move-to");
                }

                HideOthersBlocks(positions);
            }
        }

        ui_line_move.SetActive(movimentSelection);
        txt_selected_action_info.text = movimentSelection ? "move the character!" : string.Empty;
    }

    private void HideOthersBlocks(int[] positions)
    {
        for (int i = 0; i <= Global.MAX_X_POS; i++)
        {
            if (!positions.Contains(i))
                UIHelper.GetGroundBlockByIndex(i).GetComponent<Animator>().Play("block-idle");
        }
    }

    private void HideOthersBlocks(int moveBlock, int[] positions)
    {
        for (int i = 0; i < Global.MAX_X_POS; i++)
        {
            if (i != moveBlock && !positions.Contains(i))
                UIHelper.GetGroundBlockByIndex(i).GetComponent<Animator>().Play("block-idle");
        }
    }




}
