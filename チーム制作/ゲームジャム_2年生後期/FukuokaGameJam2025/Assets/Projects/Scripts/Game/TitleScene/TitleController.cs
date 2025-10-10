using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    FadeOut _fadeOut = null;

    [SerializeField] private SEController seSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            _fadeOut.StartFadeOut();
            seSound.Play();
        }
    }
}
