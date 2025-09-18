using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameMode { Normal, Zombie}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public IGameStateBase currentGameState;

    [Header("------------Variable------------")]
    public float playerTime;
    public GameMode currentMode = GameMode.Normal;

    public bool IsGameStateLoading() => currentGameState is LoadingGameState;
    public bool IsGameStateHome() => currentGameState is HomeGameState;
    public bool IsGameStatePlay() => currentGameState is PlayGameState;
    public bool IsGameStatePause() => currentGameState is PauseGameState;
    public bool IsGameStateEnd() => currentGameState is GameEndState;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        ChangeStateLoadingGame();
    }

    private void Update() {
        currentGameState?.Update(this);
    }

    public void ChangeState(IGameStateBase gameStateBase) {
        currentGameState?.Exit(this);
        currentGameState = gameStateBase;
        currentGameState.Enter(this);
    }

    public void ChangeStateLoadingGame() {
        ChangeState(new LoadingGameState());
    }

    public void ChangeStateHomeGame() {
        ChangeState(new HomeGameState());
    }

    public void ChangeStateStartGame() {
        StartCoroutine(setPlay());
    }

    public IEnumerator setPlay() {
        if(currentMode == GameMode.Normal) yield return new WaitForSeconds(0.5f);
        else if(currentMode == GameMode.Zombie) yield return null;
        ChangeState(new PlayGameState());
    }

    public void ChangeStatePauseGame() {
        ChangeState(new PauseGameState());
    }

    public void ChangeStateResumeGame() {
        ChangeState(new PlayGameState());
    }

    public void ChangeStateEndGame() {
        ChangeState(new GameEndState());
    }

    public void ChangeGameModeNormal() {
        currentMode = GameMode.Normal;
    }

    public void ChangeGameModeZombie() {
        currentMode = GameMode.Zombie;
    }

    public void ReLoadScene() {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        ChangeStateHomeGame();
        SceneManager.sceneLoaded -= OnSceneLoaded; // bỏ đăng ký để tránh lặp
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += OnMainSceneLoad;
        SceneManager.LoadScene(sceneName);
    }

    public void OnMainSceneLoad(Scene scene, LoadSceneMode mode) {
        if(scene.name == "MainScene") {
            ChangeGameModeNormal();
            ChangeStateHomeGame();
        } else if(scene.name == "LoadingZombieMode") {
            ChangeStateLoadingGame();
            ChangeGameModeZombie();
        } else if(scene.name == "ZombieCity") {
            ChangeStateHomeGame();
        }
        SceneManager.sceneLoaded -= OnMainSceneLoad;
    }
}


