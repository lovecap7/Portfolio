using UnityEngine;

public class MovingGround : MonoBehaviour
{
    public float moveDistance = 2f;     // 移動する高さ
    public float moveSpeed = 2f;        // 移動スピード
    private Vector3 startPos;
    private bool goingUp = true;

    void Start()
    {
        // 初期位置を記録
        startPos = transform.position;
    }

    void Update()
    {
        // 上下移動の方向に応じて新しい位置を計算
        float moveStep = moveSpeed * Time.deltaTime;
        if (goingUp)
        {
            transform.position += new Vector3(0, moveStep, 0);
            if (transform.position.y >= startPos.y + moveDistance)
                goingUp = false;
        }
        else
        {
            transform.position -= new Vector3(0, moveStep, 0);
            if (transform.position.y <= startPos.y)
                goingUp = true;
        }
    }
}
