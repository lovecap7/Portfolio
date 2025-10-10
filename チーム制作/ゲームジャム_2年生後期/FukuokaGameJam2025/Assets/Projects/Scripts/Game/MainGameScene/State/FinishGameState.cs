using mySystem;
using mySystem.AI.FSM;
using UnityEngine;


public class FinishGameState : State<MainGameSceneController>
{
    public FinishGameState(StateMachine<MainGameSceneController> owner) : base(owner)
    {
    }

    float _finishTime = 2.0f;
    float _timer = 0.0f;

    public override void OnEnter()
    {
        _timer = 0.0f;

        Context?.PlayFinishUIAnim();

        // カメラを止める
        Camera mainCamera = Camera.main;
        camera cam = mainCamera.GetComponent<camera>();
        if (cam != null)
        {
            cam.StopScrolling();
        }

        // 全プレイヤーを取得
        var playerList = GameObject.FindGameObjectsWithTag(TagName.Player);
        int playerIndex = 1;
        //ゴール
        float goalX = GameObject.Find("GoalArea").transform.position.x;
        //順位
        for(int i = 0; i < playerList.Length; i++)
        {
            if (!playerList[i])
            {
                continue;
            }
            var playerScript1 = playerList[i].GetComponent<Player>();
            float length1 = goalX - playerScript1.GetPos().x;
            int minRank = playerList.Length;
            int nowRank = minRank;
            for (int j = 1; j < playerList.Length; j++)
            {
                if (!playerList[j])
                {
                    continue;
                }
                var playerScript2 = playerList[j].GetComponent<Player>();
                float length2 = goalX - playerScript2.GetPos().x;
                //もしも他の人よりゴールに近いならランキングをあげる
                if(length1 < length2)
                {
                    --nowRank;
                }
            }
            //結果
            playerScript1.m_rank = nowRank;
        }

        foreach (var player in playerList)
        {
            if (!player)
            {
                continue;
            }

            // TODO : プレイヤーのリザルト情報を追加する
            GameInstance gameInst = (GameInstance)GameInstanceBase.Instance;
            if (gameInst)
            {
                PlayerResultData data = new PlayerResultData();
                var playerScript = player.GetComponent<Player>();
                var sprites = playerScript.GetPartsSprite();
                data.finalSpriteHead = sprites[0];
                data.finalSpriteBody = sprites[1];
                data.finalSpriteLeg = sprites[2];
                data.gloalRank = playerScript.m_rank;
                data.evolution = playerScript.m_totalEvoNum;
                gameInst?.AddPlayerResultData(playerIndex, data);
            }

            // 全プレイヤーをその場に固定する
            var rigitbody2D = player.GetComponent<Rigidbody2D>();
            if (rigitbody2D)
            {
                rigitbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            // プレイヤーの入力を切る
            var playerS = player.GetComponent<Player>();
            if (playerS != null)
            {
                playerS.OnDisablePlayer();
            }

            //次のインデックスに
            ++playerIndex;
        }
    }
    public override void OnUpdate()
    {
        if (_timer >= _finishTime)
        {
            Context?.NextScene();
            return;
        }
        _timer += Time.deltaTime;
    }
    public override void OnExit()
    {

    }
}
