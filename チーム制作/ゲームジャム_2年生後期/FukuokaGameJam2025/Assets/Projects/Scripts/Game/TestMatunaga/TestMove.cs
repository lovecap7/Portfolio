using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float moveSpeed_ = 5.0f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー取得
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody2Dがアタッチされているか確認
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }
    }

    // 物理演算の更新はFixedUpdateで行うのが適切
    void FixedUpdate()
    {
        //入力を取得
        float horizontalInput = Input.GetAxis("Horizontal");

        //(速度) = (入力方向) * (設定速度)
        Vector2 movement = new Vector2(horizontalInput * moveSpeed_, rb.velocity.y);

        // 3. Rigidbody2Dに新しい速度を設定し、移動させる
        rb.velocity = movement;
    }
}
