using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour

{
    [Header("スクロール設定")]
    public float startDelay = 2f;          // 開始前の待機時間
    public float scrollSpeed = 2f;         // 初期スクロール速度
    public float speedIncrease = 0.5f;     // 加速幅
    public float increaseInterval = 10f;   // 何秒ごとに加速するか

    [Header("ゴール設定")]
    public Transform goal;                 // ゴールオブジェクト
    public float goalOffset = 6f;          // ゴール可視後に追加移動する距離
    public float deceleration = 1f;        // ゴール可視後の減速速度

    private float timer = 0f;
    private bool isScrolling = false;
    private bool goalVisible = false;
    private float movedAfterGoal = 0f;
    private bool isStopped = false;

    void Start()
    {
        Invoke(nameof(StartScrolling), startDelay);
    }

    void Update()
    {
        if (!isScrolling || isStopped) return;

        float moveThisFrame = scrollSpeed * Time.deltaTime;

        // ゴール可視後は減速
        if (goalVisible)
        {
            scrollSpeed -= deceleration * Time.deltaTime;
            if (scrollSpeed < 0.1f) scrollSpeed = 0.1f; // 最低速度制限
            moveThisFrame = scrollSpeed * Time.deltaTime;
        }

        transform.position += Vector3.right * moveThisFrame;

        // 通常スクロール時の加速
        if (!goalVisible)
        {
            timer += Time.deltaTime;
            if (timer >= increaseInterval)
            {
                scrollSpeed += speedIncrease;
                timer = 0f;
            }
        }

        // ゴールが画面に映ったかチェック
        if (!goalVisible && goal != null && IsGoalVisible())
        {
            goalVisible = true;
            movedAfterGoal = 0f;
            Debug.Log("ゴールが見えた！減速開始");
        }

        // ゴール可視後の6ユニット移動判定
        if (goalVisible)
        {
            movedAfterGoal += moveThisFrame;
            if (movedAfterGoal >= goalOffset)
            {
                isStopped = true;
                Debug.Log("カメラ停止（ゴール可視後6ユニット移動完了）");
            }
        }
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }
    public void StopScrolling()
    {
        isStopped = true;
    }

    bool IsGoalVisible()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(goal.position);
        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f &&
               viewportPos.z > 0f;
    }
}