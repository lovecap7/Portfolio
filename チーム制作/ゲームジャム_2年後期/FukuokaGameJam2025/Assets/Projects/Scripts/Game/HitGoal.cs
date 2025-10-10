using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitGoal : MonoBehaviour
{
    [SerializeField] private FadeOut fadeOut;

    // Box Colliderなどにチェックが入っているときに呼ばれる（衝突判定）
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーなど、特定のオブジェクトとの接触のみを処理する場合
        if (other.gameObject.CompareTag(TagName.Player))
        {
            StartTransition();
        }
    }

    private void StartTransition()
    {
        fadeOut.StartFadeOut();
    }

    // シーン遷移を遅延実行するためのコルーチン
    private System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
