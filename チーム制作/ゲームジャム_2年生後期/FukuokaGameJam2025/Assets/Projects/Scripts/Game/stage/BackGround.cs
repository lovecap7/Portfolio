using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private Transform cam; // カメラ
    [SerializeField] private float backgroundWidth = 20f; // 背景1枚の幅
    [SerializeField] private Transform[] backgrounds; // 背景3枚をセット

    private void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            float dist = cam.position.x - bg.position.x;

            // カメラが右に進んで背景から1枚分以上離れたらループ
            if (dist > backgroundWidth)
            {
                // 一番右の背景を探す
                Transform rightMost = backgrounds[0];
                foreach (Transform other in backgrounds)
                {
                    if (other.position.x > rightMost.position.x)
                        rightMost = other;
                }

                // この背景を右端に移動
                bg.position = new Vector3(rightMost.position.x + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }
}