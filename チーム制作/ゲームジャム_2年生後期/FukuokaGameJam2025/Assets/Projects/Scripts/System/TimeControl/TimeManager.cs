using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerText; // ゲーム中のタイマー表示用
    private float elapsedTime = 0f;
    private bool isRunning = true;

    // クリア時の最終時間を保持する static 変数
    public static float finalTime = 0f;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // タイマー停止時に最終時間を保存する
    public void StopTimer()
    {
        isRunning = false;
        finalTime = elapsedTime;
    }
    
}