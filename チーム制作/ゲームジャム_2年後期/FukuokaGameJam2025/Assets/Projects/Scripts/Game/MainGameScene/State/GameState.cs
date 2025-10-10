using mySystem.AI.FSM;
using UnityEngine;


public class GameState : State<MainGameSceneController>
{
    public GameState(StateMachine<MainGameSceneController> owner) : base(owner)
    {
    }

    float _timer = 0.0f;

    public override void OnEnter()
    {
        _timer = 0.0f;
        var playerList = GameObject.FindGameObjectsWithTag(TagName.Player);
        foreach (var player in playerList)
        {
            var playerS = player.GetComponent<Player>();
            if (playerS != null)
            {
                playerS.OnEnablePlayer();
            }
        }

    }
    public override void OnUpdate()
    {
        // ゲーム終了のデバッグコード
        if (Input.GetKeyDown(KeyCode.N))
        {
            Context.OnGoalEvent();
            return;
        }
        _timer += Time.deltaTime;
    }
    public override void OnExit()
    {

    }
}
