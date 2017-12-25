using Assets.Code;
using Assets.Code.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundBlock : MonoBehaviour {

    public int Position;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        var positions = Move.MovablePositions(
            Global.BattleManager.Battle.Player.Position, 
            Global.BattleManager.Battle.Enemy.Position, 
            Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MinRange, 
            Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MaxRange);

        if (Global.BattleManager.Battle.State == Assets.Code.Battle.BattleState.ActionRelatedInput && positions.Contains(Position))
        {
            Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MoveTo = Position;
        }   
    }


}
