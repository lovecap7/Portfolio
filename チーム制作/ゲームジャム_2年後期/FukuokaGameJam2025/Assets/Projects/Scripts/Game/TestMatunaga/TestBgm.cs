using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBgm : MonoBehaviour
{
    [SerializeField] private AudioClip bgmSound;

    // Start is called before the first frame update
    void Awake()
    {
        SoundManager.Instance.PlayBGM(bgmSound);
    }
}
