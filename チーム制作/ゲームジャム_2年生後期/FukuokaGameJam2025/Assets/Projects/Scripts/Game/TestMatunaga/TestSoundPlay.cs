using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundPlay : MonoBehaviour
{
    [SerializeField]private AudioClip seSound;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーなど、特定のオブジェクトとの接触のみを処理する場合
        if (other.gameObject.CompareTag(TagName.Player))
        {
            SoundManager.Instance.PlaySE(seSound);
        }
    }
}
