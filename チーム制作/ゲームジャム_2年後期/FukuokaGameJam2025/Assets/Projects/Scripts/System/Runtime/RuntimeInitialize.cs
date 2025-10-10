using UnityEngine;

namespace mySystem
{
    public class RuntimeInitialize
    {
        static readonly int FPS = 60;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
#if UNITY_EDITOR
            Debug.Log("Setup System");
#else
            Debug.unityLogger.logEnabled = false;
#endif
            var gameConfigData = Resources.Load<GameConfig>("SO_GameConfig");
            if (gameConfigData == null)
            {
                Debug.LogError("GameConfig が存在しません");
                return;
            }

            // GameManager を生成
            if (gameConfigData.GameInstance)
            {
                GameInstanceBase gameInstance = Object.Instantiate(gameConfigData.GameInstance);
                gameInstance.name = gameConfigData.GameInstance.name;
                Object.DontDestroyOnLoad(gameInstance);
            }

            // フレームレート初期化
            Application.targetFrameRate = FPS;
        }
    }
}
