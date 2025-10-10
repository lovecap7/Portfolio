using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName = "Game_Matunaga";

   public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void LoadScene2HitPlayer4Goal(string _name,Collider2D _goal)
    {
        if (_goal.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(_name);
        }
    }
}
