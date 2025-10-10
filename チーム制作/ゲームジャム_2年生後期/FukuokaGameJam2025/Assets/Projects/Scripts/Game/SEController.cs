using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEController : MonoBehaviour
{
    [SerializeField] private AudioSource seSound;

    // Start is called before the first frame update
    public void Play()
    {
        //çƒê∂
        seSound.PlayOneShot(seSound.clip);
    }
}
