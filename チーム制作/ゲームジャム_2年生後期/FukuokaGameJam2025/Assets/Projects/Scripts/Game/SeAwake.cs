using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeAwake : MonoBehaviour
{
    [SerializeField] private AudioSource seSound;
    public void Awake()
    {
        //çƒê∂
        seSound.PlayOneShot(seSound.clip);
    }
}
