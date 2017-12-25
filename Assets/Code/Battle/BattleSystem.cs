using Assets.Code.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Battle
{

    public enum BattleState
    {
        NotStarted = 0,
        Intro = 1,
        ActionInput = 2,
        ActionRelatedInput = 3,
        TurnStarted = 4,
        TurnEnded = 5,
        BattleEnded = 6
    }

    public class BattleSystem
    {
        public BattleSystem()
        {
            State = BattleState.NotStarted;
        }

        public delegate void BattleStateChange(BattleState previousState, BattleState actualState);

        public event BattleStateChange OnChangeState;

        public event BattleStateChange OnIntroBegin;

        public event EventHandler OnActionSelected;

        public event EventHandler OnTurnIsReady;

        private BattleState state = BattleState.NotStarted;

        public int Round { get; set; }

        public int Turn { get; set; }

        public Character Player { get; set; }

        public void StartNewBattle()
        {
            this.State = BattleState.ActionInput;
            this.Player.HP = Global.MAX_HP;
            this.Player.Position = Global.PlayerStartPosition;

            this.Enemy.HP = Global.MAX_HP;
            this.Enemy.Position = Global.OponentStartPosition;
            this.Turn = 1;
            this.Round = 1;
        }

        public Character Enemy { get; set; }

        public BattleState State
        {
            get
            {
                return state;
            }

            private set
            {
                BattleStateChange handler = OnChangeState;

                if (handler != null)
                {
                    handler(this.State, value);
                }

                if (value == BattleState.Intro && this.State != BattleState.Intro)
                {
                    if (OnIntroBegin != null)
                    {
                        OnIntroBegin(this.State, value);
                    }
                }

                state = value;
            }
        }

        public List<BattleTurn> BattleTurns { get; private set; }

        public void NewBattle(Character player, Character enemy)
        {
            this.Player = player;
            this.Enemy = enemy;
            this.State = BattleState.Intro;
            this.BattleTurns = new List<BattleTurn>();

            CreateNewTurn();

        }

        private void CreateNewTurn()
        {
            BattleTurns.Add(new BattleTurn((this.BattleTurns.Count + 1)));
        }

        public BattleTurn CurrentTurn
        {
            get
            {
                return BattleTurns.Last();
            }
        }

        //public int SelectedPosition { get; internal set; }

        public void SetTurnAction(BaseAction action, CharacterType author)
        {

            if (author == CharacterType.Player)
            {
                SetPlayerBaseAction(action);
            }
            else
            if (author == CharacterType.Oponent)
            {
                this.CurrentTurn.OponentBaseAction = action;
            }

            if (this.CurrentTurn.CheckIfPlayerTurnIsReady())
            {
                if (OnTurnIsReady != null)
                {
                    this.OnTurnIsReady(this, null);
                }
            }

        }

        private void SetPlayerBaseAction(BaseAction action)
        {
            CurrentTurn.PlayerBaseAction = action;

            if (action.HasToMove)
            {
                this.State = BattleState.ActionRelatedInput;
                action.MoveTo = Move.NextPositionForward(this.Player.Position, this.Enemy.Position, action.MinRange, action.MaxRange);
            }
            else
            {
                action.MoveTo = this.Player.Position;
                this.State = BattleState.ActionInput;
            }

            if (OnActionSelected != null)
            {
                OnActionSelected(this, null);
            }
        }

        public void StartTheTurn()
        {

            this.OponenAI();

            this.TurnExecution();

            this.State = BattleState.TurnStarted;

        }

        private void TurnExecution()
        {
            //0.check priority
            //1.active play atack
            //2.attive the before-activation-actions of the  active-players 
            //3.check if the reative player is in the range of the attack
            //  3.1 if so, execute the strike (it should regard the Soak and Stun Guard value) 

            //  3.reactive-player =>  on-hit actions 
            //4.active-player: after-activation actions
            //

            var priority = CurrentTurn.PriorityOwner;

            var activePlayer = priority == CharacterType.Player ? this.Player : this.Enemy;
            var reactivePlayer = priority == CharacterType.Player ? this.Enemy : this.Player;

            var activePlayerAction = priority == CharacterType.Player ? CurrentTurn.PlayerBaseAction : CurrentTurn.OponentBaseAction;
            var reactivePlayerAction = priority == CharacterType.Player ? CurrentTurn.OponentBaseAction : CurrentTurn.PlayerBaseAction;


            var steps = new List<ActionStep>();

            ActionSequence(activePlayer, reactivePlayer, activePlayerAction, steps);

            ActionSequence(reactivePlayer, activePlayer, reactivePlayerAction, steps);

            this.CurrentTurn.ActionSteps = steps;

        }

        public void CheckIfTheTurnEnded()
        {

            if (this.State == BattleState.TurnStarted) {
                if (this.CurrentTurn.ActionSteps.Any(o => o.Performed == false) == false)
                {
                    this.CreateNewTurn();

                    this.State = BattleState.ActionInput;
                }
            }
            
        }

        private static void ActionSequence(Character activePlayer, Character reactivePlayer, BaseAction activePlayerAction, List<ActionStep> steps)
        {

            steps.Add(new ActionStep { Author = activePlayer, StepMessage = activePlayer.Name.ToString() + ":look-at" });


            var damage = activePlayerAction.Power;
            
                var step = new ActionStep
                {
                    Author = activePlayer,
                    StepMessage = activePlayer.Name.ToString() + ":hit",
                    ActionRef = activePlayerAction,
                    StepType = ActionStepType.Attack
                };

                step.Run = delegate ()
                {

                    if (BattleHelper.CheckAttackRange(activePlayer.Position, reactivePlayer.Position, activePlayerAction.MinRange, activePlayerAction.MaxRange))
                    {
                        reactivePlayer.HP -= damage;
                    }

                    activePlayer.Position = activePlayerAction.MoveTo;
                  
                };

                steps.Add(step);
            
        }

        private void OponenAI()
        {

            if (Math.Abs(this.Player.Position - this.Enemy.Position) > 3) {
                this.CurrentTurn.OponentBaseAction = Global.BaseActions.GetByName("dash");
            }
            else
            {
                this.CurrentTurn.OponentBaseAction = Global.BaseActions.GetRandomly();
            }

           this.CurrentTurn.OponentBaseAction.MoveTo = Enemy.Position;

            if (this.CurrentTurn.OponentBaseAction.HasToMove)
            {

                var positions = Move.MovablePositions(Global.BattleManager.Battle.Enemy.Position, Global.BattleManager.Battle.CurrentTurn.PlayerBaseAction.MoveTo, CurrentTurn.OponentBaseAction.MinRange, CurrentTurn.OponentBaseAction.MaxRange);

                var p1 = Math.Min(positions.First(), positions.Last());
                var p2 = Math.Max(positions.First(), positions.Last());


                int pos = Global.Random.Next(p1, p2);

                this.CurrentTurn.OponentBaseAction.MoveTo = pos;

            }

        }


    }

    public enum ActionStepType
    {
        Log = 0,
        Movement = 1,
        Attack = 2
    }

    public class ActionStep
    {
        internal Action Run;

        public bool Performed { get; set; }
        public Character Author { get; internal set; }

        public string StepMessage { get; internal set; }

        public ActionStepType StepType { get; internal set; }
        public BaseAction ActionRef { get; internal set; }

        public bool OnRange(int oponentPosition) {
            return BattleHelper.CheckAttackRange(this.Author.Position, oponentPosition, this.ActionRef.MinRange, this.ActionRef.MaxRange);
        }


    }
}

