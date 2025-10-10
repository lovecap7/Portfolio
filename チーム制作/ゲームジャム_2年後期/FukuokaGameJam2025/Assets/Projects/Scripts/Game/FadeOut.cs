using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{

    public string nextScene = "NextScene";
    private Image targetImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        // このスクリプトがアタッチされているGameObjectからImageコンポーネントを取得
        targetImage = GetComponent<Image>();

        // 念のため、取得できたか確認
        if (targetImage == null)
        {
            Debug.LogError("FadeController: Imageコンポーネントが見つかりませんでした。このスクリプトはImageコンポーネントを持つGameObjectにアタッチしてください。");
            CanvasGroup canvasGroup = targetImage.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                // 2. クリック入力をブロックしないように設定
                //    これにより、下のボタンが押せるようになる
                canvasGroup.blocksRaycasts = false;

                // (おまけ) 完全に透明になったらGameObjectを非アクティブにするのも一般的です
                targetImage.gameObject.SetActive(false);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (targetImage != null)
        {
            // Imageの色を取得
            Color color = targetImage.color;

            color.a = 0.0f;

            // Imageに新しい色（透明な状態）を適用
            targetImage.color = color;
        }
    }

    public void StartFadeOut()
    {
        if (targetImage != null)
        {
            StartCoroutine(FadeOutCoroutine(targetImage, fadeDuration));
        }
    }

    private IEnumerator FadeOutCoroutine(Image image, float duration)
    {
        float startAlpha = image.color.a; // 開始時のアルファ値 (通常は1.0)
        float targetAlpha = 1.0f;          // 終了時のアルファ値 (透明)
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration; // 0から1へ変化する割合

            // アルファ値を線形補間 (Lerp) で計算
            // 開始値(1.0)から終了値(0.0)へ徐々に変化
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);

            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;

            yield return null; // 1フレーム待機
        }

        SceneManager.LoadScene(nextScene);

    }

   
}
