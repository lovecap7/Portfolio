using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource = null;

    private void Awake()
    {
        bgmSource.loop = true; // BGMはループ設定をONにする
        bgmSource.Play();
    }

    private void OnDestroy()
    {
        bgmSource.Stop();
        bgmSource.clip = null; // 次回再生のためにクリップをクリア
    }
}
