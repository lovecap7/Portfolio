using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

public class ResultAdvance : MonoBehaviour
{
    [SerializeField] private FadeOut fade;      // FadeOutコンポ
    [SerializeField] private AudioSource se;    // 押下SE（任意）
    [SerializeField] private string nextScene;  // 例: "S_Title"

    bool transitioning;
    float blockUntil;

    void Awake()
    {
        if (!fade) fade = FindObjectOfType<FadeOut>(true);
        if (!se)   se   = GetComponent<AudioSource>();
        if (string.IsNullOrEmpty(nextScene) && fade != null)
            nextScene = fade.nextScene; // インスペクタ未設定でも拾う
    }

    void OnEnable()
    {
        blockUntil = Time.unscaledTime + 0.2f; // 誤爆防止
    }

    void Update()
    {
        if (transitioning || Time.unscaledTime < blockUntil) return;

        #if ENABLE_INPUT_SYSTEM
        if (PressedThisFrame_NewInput())
            TryAdvance();
        #else
        if (Input.anyKeyDown)
            TryAdvance();
        #endif
    }

    #if ENABLE_INPUT_SYSTEM
    bool PressedThisFrame_NewInput()
    {
        // --- Keyboard ---
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        // --- Mouse ---
        var m = Mouse.current;
        if (m != null && (m.leftButton.wasPressedThisFrame
                       || m.rightButton.wasPressedThisFrame
                       || m.middleButton.wasPressedThisFrame))
            return true;

        // --- Gamepad (全部のボタンを総なめ) ---
        foreach (var pad in Gamepad.all)
        {
            if (pad == null) continue;
            // 代表的なボタンをチェック（十分網羅）
            if (pad.startButton.wasPressedThisFrame
             || pad.selectButton.wasPressedThisFrame
             || pad.buttonSouth.wasPressedThisFrame  // A / Cross
             || pad.buttonEast.wasPressedThisFrame   // B / Circle
             || pad.buttonWest.wasPressedThisFrame   // X / Square
             || pad.buttonNorth.wasPressedThisFrame  // Y / Triangle
             || pad.leftShoulder.wasPressedThisFrame
             || pad.rightShoulder.wasPressedThisFrame
             || pad.leftStickButton.wasPressedThisFrame
             || pad.rightStickButton.wasPressedThisFrame
             || pad.dpad.up.wasPressedThisFrame
             || pad.dpad.down.wasPressedThisFrame
             || pad.dpad.left.wasPressedThisFrame
             || pad.dpad.right.wasPressedThisFrame)
                return true;
        }
        return false;
    }
    #endif

    void TryAdvance()
    {
        transitioning = true;
        if (se) se.Play();

        // あなたの FadeOut のAPI名に合わせて↓を差し替え
        // 例: fade.StartFadeOut(); その中で LoadScene する想定
        // もし即ロードでOKなら以下1行を生かす
        if (fade == null && !string.IsNullOrEmpty(nextScene))
            SceneManager.LoadScene(nextScene);
        else
        {
            // ここ、あなたのFadeOut実装に合わせて呼び出してね
            // 例) fade.Begin(); / fade.Play(); / fade.StartFadeOut();
        }
    }
}
