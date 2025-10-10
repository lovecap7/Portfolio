using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;      // GameOverPanel (Canvas下)
    [SerializeField] private string gameOverSceneName = "TitleScene"; // 自動遷移先（空文字で無効）
    [SerializeField] private float autoTransitionDelay = 2f; // 自動遷移までの遅延（秒）。0で即
    [SerializeField] private float fadeDuration = 0.5f;  // フェード時間
    [SerializeField] private bool pauseOnShow = false;   // trueにすると表示後にTime.timeScale=0で一時停止

    public static GameOverManager Instance { get; private set; }

    private CanvasGroup cg;
    private bool shown = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
            cg = gameOverUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = gameOverUI.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    // 外部から呼ぶメソッド
    public void ShowGameOver()
    {
        if (shown) return;
        shown = true;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            // 前面に確実に出したければ
            gameOverUI.transform.SetAsLastSibling();
            StartCoroutine(FadeInAndMaybePause());
        }
        else
        {
            // UIが未設定でも自動遷移は行う
            if (!string.IsNullOrEmpty(gameOverSceneName))
                StartCoroutine(TransitionAfterDelayRealtime(autoTransitionDelay));
        }
    }

    private IEnumerator FadeInAndMaybePause()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // UIのフェードはunscaledで行う（Time.timeScaleの影響受けない）
            cg.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        if (pauseOnShow)
        {
            Time.timeScale = 0f;
        }

        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            StartCoroutine(TransitionAfterDelayRealtime(autoTransitionDelay));
        }
    }

    private IEnumerator TransitionAfterDelayRealtime(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        // まず Time.timeScale を戻す（安全のため）
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    // ボタンから呼び出す用（InspectorでOnClickにセット）
    public void GoToTitleNow()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(gameOverSceneName))
            SceneManager.LoadScene(gameOverSceneName);
    }
}