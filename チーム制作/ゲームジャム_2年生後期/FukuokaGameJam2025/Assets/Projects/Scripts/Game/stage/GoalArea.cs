using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    [SerializeField]
    MainGameSceneController mainGameSceneController;

    [SerializeField] private SEController seSound;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("コリジョンがない");
        }
    }

    // Box Colliderなどにチェックが入っているときに呼ばれる（衝突判定）
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーなど、特定のオブジェクトとの接触のみを処理する場合
        if (other.gameObject.CompareTag(TagName.Player))
        {
            mainGameSceneController?.OnGoalEvent();
            seSound.Play();
        }
    }
}
