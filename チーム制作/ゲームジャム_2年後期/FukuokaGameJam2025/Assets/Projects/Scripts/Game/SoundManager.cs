using mySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioSource seSource = null;
    [SerializeField]private AudioSource bgmSource = null;

    [Range(0f,1f)]private float seVolume = 1f;
    [Range(0f,1f)]private float bgmVolume = 1f;

    protected override void Awake()
    {
        base.Awake();

        if (seSource != null) seSource.volume = seVolume;
        if (bgmSource != null)bgmSource.volume = bgmVolume;
    }


    public void PlaySE(AudioClip clip)
    {
        if (seSource == null || clip == null) return;
        //再生
        seSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;

        // 現在と同じBGMなら何もしない
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true; // BGMはループ設定をONにする
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            bgmSource.clip = null; // 次回再生のためにクリップをクリア
        }
    }

    public void AdjustVolumeSE(float volume)
    {
        // 0.0f～1.0fの範囲にクランプする
        seVolume = Mathf.Clamp01(volume);

        if (seSource != null)
        {
            // 💡 AudioSourceのvolumeプロパティに値を設定
            // PlayOneShotで再生される音も、このvolume設定に従います
            seSource.volume = seVolume;
        }
    }
    public void AdjustVolumeBGM(float volume)
    {
        // 0.0f～1.0fの範囲にクランプする
        bgmVolume = Mathf.Clamp01(volume);

        if (bgmSource != null)
        {
            // 💡 AudioSourceのvolumeプロパティに値を設定
            // PlayOneShotで再生される音も、このvolume設定に従います
            bgmSource.volume = bgmVolume;
        }
    }
}
