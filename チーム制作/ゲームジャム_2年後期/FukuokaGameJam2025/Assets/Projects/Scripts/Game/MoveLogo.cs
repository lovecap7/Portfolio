using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLogo : MonoBehaviour
{
    [SerializeField] private float moveMax = 10.0f;
    [SerializeField] private float moveSpeed = 0.25f;

    float pow = 0.0f;

    private RectTransform targetRectTransform;

    private void Start()
    {
        targetRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos.y += moveSpeed * Time.deltaTime;
        targetRectTransform.position = pos;

        pow += Mathf.Abs(moveSpeed);
        if(pow> moveMax)
        {
            pow = 0.0f;

            moveSpeed *= -1.0f;
        }
    }
}
