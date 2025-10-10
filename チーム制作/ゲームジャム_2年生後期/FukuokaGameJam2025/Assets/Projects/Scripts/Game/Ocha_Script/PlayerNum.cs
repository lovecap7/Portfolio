using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNum : MonoBehaviour
{
    const int MaxplayerNum = 4;
    const int MinplayerNum = 1;
    public static int playerNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void Add()
    {
        if(MaxplayerNum <= playerNum) return;
        playerNum++;
        Debug.Log(playerNum);
    }
    public void Sub()
    {
        if(MinplayerNum >= playerNum) return;
        playerNum--;
        Debug.Log(playerNum);
    }
}
