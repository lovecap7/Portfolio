using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingBtn : MonoBehaviour
{
    [SerializeField] private float scaleMax = 0.2f;
    [SerializeField] private float scaleAcc = 0.005f;

    float pow = 0.0f;

    private RectTransform targetRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        targetRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;

        scale.x += scaleAcc * Time.deltaTime;
        scale.y += scaleAcc * Time.deltaTime;
        scale.z += scaleAcc * Time.deltaTime;
        targetRectTransform.localScale = scale;

        pow += Mathf.Abs(scaleAcc);
        if (pow > scaleMax)
        {
            pow = 0.0f;

            scaleAcc *= -1.0f;
        }
    }
}
