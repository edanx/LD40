using Assets.Code;
using Assets.Code.Battle;
using Assets.Code.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    public SfxHelper SFX;

    public BattleSystem Battle;

    public Transform Player;

    public Transform Enemy;

    public Transform UI_ActionSelection;

    public Transform UI_Intro;

    public Transform IntroDialogue;

    public Button ButtonStartTurn;

    public Transform[] Blocks;

    public Transform DefaultCameraTarget;

    private Transform CameraTarget;

    public Transform Camera;

    EfxManager EFX;

    private void Awake()
    {


        Battle = new BattleSystem();

        Battle.OnChangeState += Battle_OnChangeState;
        Battle.OnIntroBegin += Battle_OnIntroBegin;

        Global.BattleManager = this;

        EFX = GameObject.Find("EfxManager").GetComponent<EfxManager>();
        SFX = GameObject.Find("SfxManager").GetComponent<SfxHelper>();
    }


    private void Battle_OnIntroBegin(BattleState previousState, BattleState actualState)
    {
        var dialogue = this.IntroDialogue.GetComponent<DialogueImplementation>().defaultDialogue;

        this.IntroDialogue.GetComponent<Dialogue>().Run(dialogue);
    }

    private void AfterIntro()
    {
        this.Battle.StartNewBattle();
    }

    private void Battle_OnChangeState(BattleState previousState, BattleState actualState)
    {

        switch (actualState)
        {
            case BattleState.NotStarted:
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(false);
                CameraResetTarget();
                break;
            case BattleState.Intro:
                UI_Intro.gameObject.SetActive(true);
                UI_ActionSelection.gameObject.SetActive(false);
                CameraResetTarget();
                break;
            case BattleState.ActionInput:
                UIHelper.HideAllBlocks();
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(true);
                CameraResetTarget();
                break;
            case BattleState.ActionRelatedInput:
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(true);
                break;
            case BattleState.TurnStarted:
                UIHelper.HideAllBlocks();
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(false);

                StartCoroutine(TurnExecution());
                break;
            case BattleState.TurnEnded:
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(false);
                break;
            case BattleState.BattleEnded:
                UI_Intro.gameObject.SetActive(false);
                UI_ActionSelection.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    void Start()
    {


        var enemy = new Character
        {
            Name = Enemy.GetComponent<Enemy>().Name,
            HP = Enemy.GetComponent<Enemy>().HP,
            Type = CharacterType.Oponent
        };

        Battle.NewBattle(Global.PlayerData, enemy);


    }

    IEnumerator TurnExecution()
    {

        foreach (var step in this.Battle.CurrentTurn.ActionSteps)
        {
            var author = step.Author.Type == CharacterType.Player ? this.Player : this.Enemy;

            //CameraLookAt(step.Author);

            if (step.ActionRef != null)
            {
                if (step.ActionRef.MoveTo != step.Author.Position)
                {
                    var pos = UIHelper.GetGroundBlockByIndex(step.ActionRef.MoveTo);
                    SFX.PlaySFX("jump");
                    yield return (MoveToPosition(author, pos.transform.position, 0.5f));
                }

                if ((step.StepType == ActionStepType.Attack) && step.ActionRef.HasToMove == false)
                {

                    var strike_efx = Instantiate(EFX.EfxStrike);

                    strike_efx.transform.position = author.position;

                    var scale = author.lossyScale;

                    scale.x *= -1;
                    strike_efx.transform.localScale = scale;
                }
            }

            if (step.StepType == ActionStepType.Attack && !step.ActionRef.HasToMove)
            {

                var oponentPosition = step.Author.Type == CharacterType.Oponent ? this.Battle.Player.Position : this.Battle.Enemy.Position;

                if (step.OnRange(oponentPosition))
                {
                    ShowHitText("-" + step.ActionRef.Power, oponentPosition);
                    SFX.PlaySFX("attack");
                }
                else
                {
                    ShowHitText("MISS", oponentPosition);
                }
            }

            if (step.Run != null)
            {
                
                step.Run();
                yield return new WaitForSeconds(1.5f);
            }

            step.Performed = true;

        }
    }

    private void ShowHitText( string text, int oponentPosition)
    {
        var text_efx = Instantiate(EFX.EfxTextUp);
        text_efx.transform.position = UIHelper.GetGroundBlockByIndex(oponentPosition).transform.position - new Vector3(0, -3);
        text_efx.GetComponentInChildren<TextMesh>().text = text;
    }

    IEnumerator MoveToPosition(Transform trans, Vector3 newPosition, float time)
    {

        float elapsedTime = 0;

        Vector3 startingPos = trans.position;
        while (elapsedTime < time)
        {

            trans.position = Vector3.Lerp(startingPos, newPosition, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / time)));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    private void CameraLookAt(Character author)
    {
        CameraTarget = author.Type == CharacterType.Player ? this.Player : this.Enemy;
    }

    private void CameraResetTarget()
    {
        CameraTarget = DefaultCameraTarget;
    }

    void Update()
    {
        if (this.Battle.State != BattleState.Intro && this.Battle.State != BattleState.TurnStarted)
        {
            Player.position = UIHelper.GetGroundBlockByIndex(this.Battle.Player.Position).transform.position;
            Enemy.position = UIHelper.GetGroundBlockByIndex(this.Battle.Enemy.Position).transform.position;
        }

        if (Player.transform.position.x < Enemy.transform.position.x)
        {
            TurnToRight(Player);
            TurnToLeft(Enemy);

        }
        else
        {
            TurnToLeft(Player);
            TurnToRight(Enemy);
        }

        if (this.Battle.State != BattleState.NotStarted && this.Battle.State != BattleState.Intro)
        {
            if(this.Battle.Player.HP <= 0 || this.Battle.Enemy.HP <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }



        ButtonStartTurn.gameObject.SetActive(TurnReady());

        Camera.transform.LookAt(CameraTarget);

        Battle.CheckIfTheTurnEnded();
    }

    private void TurnToRight(Transform character)
    {
        var scale = character.localScale;
        scale.x = -1.5f;
        character.localScale = scale;
    }

    private void TurnToLeft(Transform character)
    {
        var scale = character.localScale;
        scale.x = 1.5f;
        character.localScale = scale;
    }

    private bool TurnReady()
    {
        return this.Battle.CurrentTurn.CheckIfPlayerTurnIsReady();
    }

    public void SelectAnAction(string actionName)
    {
        var action = Global.BaseActions.GetByName(actionName);

        if (action != null)
        {
            this.Battle.SetTurnAction(action, CharacterType.Player);
        }
    }

    public void StartTheTurn()
    {
        this.Battle.StartTheTurn();
    }
}
