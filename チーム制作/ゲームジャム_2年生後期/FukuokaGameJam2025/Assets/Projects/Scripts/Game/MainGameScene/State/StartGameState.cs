using mySystem;
using mySystem.AI.FSM;
using UnityEngine;

public class StartGameState : State<MainGameSceneController>
{
    public StartGameState(StateMachine<MainGameSceneController> owner) : base(owner)
    {
    }

    float _delayStartAnimTime = 0.8f;
    float _startTime = 2.9f;
    float _timer = 0.0f;
    bool isStartAnim = false;
    bool isFinishAnim = false;

    public override void OnEnter()
    {
        _timer = 0.0f;
        isStartAnim = false;
        isFinishAnim = false;

        // リザルトデータリセット
        GameInstance gameInst = (GameInstance)GameInstanceBase.Instance;
        if (gameInst)
        {
            gameInst?.ResetPlayerResultData();
        }
        var playerList = GameObject.FindGameObjectsWithTag(TagName.Player);
        foreach (var player in playerList)
        {
            var playerS = player.GetComponent<Player>();
            if (playerS != null)
            {
                playerS.OnDisablePlayer();
            }
        }
    }

    public override void OnUpdate()
    {
        if (isStartAnim == false)
        {
            if (_timer >= _delayStartAnimTime)
            {
                Context.PlayStartCountUIAnim();
                isStartAnim = true;
                _timer = 0.0f;
            }

        }
        else
        {
            if (_timer >= _startTime)
            {
                if (isFinishAnim == false)
                {
                    Context.StopCountUIAnim();
                    Context.PlayStartUIAnim();
                    isFinishAnim = true;
                }
                else
                {
                    // 若干遅らせてゲームステートに遷移
                    if (_timer >= _startTime + 0.6f)
                    {
                        Owner.SendTrigger((int)(MainGameSceneController.EMainGameTransitionID.Game));
                        return;
                    }
                }
            }
        }
            _timer += Time.deltaTime;
    }
    public override void OnExit() 
    {
        Context.StopStartUIAnim();
    }
}
