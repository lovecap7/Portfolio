using UnityEngine;
using mySystem.AI.FSM;


public class MainGameSceneController : MonoBehaviour
{
    StateMachine<MainGameSceneController> _stateMachine = null;

    public enum EMainGameTransitionID
    {
        Game,
        Finish
    }
    [SerializeField]
    string _nextScene = "S_Title";
    [SerializeField]
    GameObject _startCountUI = null;
    [SerializeField]
    GameObject _startUI = null;
    [SerializeField]
    GameObject _finishUI = null;
    [SerializeField]
    FadeOut _fadeOut = null;

    public void PlayStartCountUIAnim()
    {
        _startCountUI.SetActive(true);
    }
    public void PlayStartUIAnim()
    {
        _startUI.SetActive(true);
    }
    public void StopCountUIAnim()
    {
        _startCountUI.SetActive(false);
    }
    public void StopStartUIAnim()
    {
        _startUI.SetActive(false);
    }
    public void PlayFinishUIAnim()
    {
        _finishUI.SetActive(true);
    }
    public void StopFinishUIAnim()
    {
        _finishUI.SetActive(false);
    }

    public void OnGoalEvent()
    {
        // ゴール時に FinishEvent に遷移させる
        _stateMachine?.SendTrigger((int)(MainGameSceneController.EMainGameTransitionID.Finish));
    }

    public void NextScene()
    {
        _fadeOut.nextScene = _nextScene;
        _fadeOut.StartFadeOut();
    }

    #region Unity Methots
    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new StateMachine<MainGameSceneController>(this);
        if (_stateMachine == null)
        {
            return;
        }

        _stateMachine.AddState<StartGameState>();
        _stateMachine.AddState<GameState>();
        _stateMachine.AddState<FinishGameState>();

        _stateMachine.AddTransition<StartGameState, GameState>((int)(EMainGameTransitionID.Game));
        _stateMachine.AddTransition<GameState, FinishGameState>((int)(EMainGameTransitionID.Finish));

        _stateMachine.SetStartState<StartGameState>();

        _stateMachine.OnEnable();

        _startCountUI.SetActive(false);
        _startUI.SetActive(false);
        _finishUI.SetActive(false);
    }

    private void OnDestroy()
    {
        _stateMachine?.OnDisable();
        _stateMachine = null;

    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine?.OnUpdate();
    }
    #endregion
}
