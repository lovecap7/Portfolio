using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Index : MonoBehaviour
{
   [SerializeField] GameObject Gauge;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerNum.playerNum; i++)
        {
            Instantiate(Gauge, new Vector2(200,200), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
