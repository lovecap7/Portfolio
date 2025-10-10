using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2.0f;         // 移動速度
    public float moveDistance = 2.0f;      // 上下移動の距離

    private Vector3 startPos;
    private bool goingUp = false;          // false で「下向き」からスタート

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float moveStep = moveSpeed * Time.deltaTime;

        if (!goingUp)
        {
            transform.position -= new Vector3(0, moveStep, 0); // 下へ移動
            if (transform.position.y <= startPos.y - moveDistance)
                goingUp = true; // 一番下に到達したら上昇へ切り替え
        }
        else
        {
            transform.position += new Vector3(0, moveStep, 0); // 上へ移動
            if (transform.position.y >= startPos.y)
                goingUp = false; // 元の位置まで来たら再び下降へ
        }
    }
}