using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimitPlayer : MonoBehaviour
{
    [SerializeField] int currentPlayers;
    [SerializeField] PlayerInputManager player_num;
    
    [SerializeField] GameObject playerPrefab; 

    void Start()
    {
        // Player Prefabをコードで設定
        player_num.playerPrefab = playerPrefab;
    
        player_num.onPlayerJoined += HandlePlayerJoined;
    
        for (int i = 0; i < PlayerNum.playerNum; i++)
        {
            player_num.JoinPlayer(i);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void HandlePlayerJoined(PlayerInput playerInput)
    {
        currentPlayers++;

        if (currentPlayers >= PlayerNum.playerNum)
        {
            player_num.DisableJoining();
            Debug.Log("最大プレイヤー数に達しました。参加受付を停止します。");
        }
    }
 
}
