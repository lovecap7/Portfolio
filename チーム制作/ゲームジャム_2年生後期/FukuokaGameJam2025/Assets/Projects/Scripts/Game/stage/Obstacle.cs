using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float disableTime = 2f;      // 操作不能にする時間（秒）
    [SerializeField] float blinkInterval = 0.2f;  // 点滅の間隔（秒）

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;

            // プレイヤーのスクリプトを一時的に無効化
            MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script.enabled && script != this) // Obstacle 自身は除外
                {
                    script.enabled = false;
                    StartCoroutine(ReEnable(script, disableTime));
                }
            }

            // 子オブジェクト含む全パーツの SpriteRenderer を取得して点滅開始
            SpriteRenderer[] srs = player.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                StartCoroutine(Blink(sr, disableTime));
            }

            Debug.Log(player.name + " が障害物に接触！操作不能＋点滅");
        }
    }

    private IEnumerator ReEnable(MonoBehaviour script, float delay)
    {
        yield return new WaitForSeconds(delay);
        script.enabled = true;
        Debug.Log(script.name + " の操作を再開しました");
    }

    private IEnumerator Blink(SpriteRenderer sr, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            sr.enabled = !sr.enabled; // 表示/非表示 切替
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        sr.enabled = true; // 最後はONに戻す
    }
}