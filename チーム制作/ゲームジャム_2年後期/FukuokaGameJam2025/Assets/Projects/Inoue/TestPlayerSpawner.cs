using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerSpawner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("TestPlayerSpawner開始");
        var manager = GetComponent<PlayerInputManager>();
        
        if (manager == null)
        {
            Debug.LogError("PlayerInputManagerが見つかりません");
            return;
        }
        
        // Control Schemeを指定せずにプレイヤーを生成
        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"プレイヤー{i + 1}を生成");
            var player = PlayerInput.Instantiate(
                manager.playerPrefab,
                playerIndex: i,
                controlScheme: null,  // nullにする
                splitScreenIndex: -1,
                pairWithDevice: null  // nullにする
            );
            
            // 位置を調整
            if (player != null)
            {
                player.transform.position = new Vector3(i * 3f, 0, 0);
            }
        }
    }
}