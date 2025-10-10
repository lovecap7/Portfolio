using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // タイトルシーンの名前を指定
    [SerializeField] private string titleSceneName = "TitleScene";

    // ボタンから呼び出すメソッド
    public void GoToTitleScene()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    // Updateでキー入力を検知する場合
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToTitleScene();
        }

        if (Input.GetButtonDown("Jump"))
        {
            GoToTitleScene();
        }
    }
}
