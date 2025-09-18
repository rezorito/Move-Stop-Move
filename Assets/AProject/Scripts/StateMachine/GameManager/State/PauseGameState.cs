using UnityEngine;

public class PauseGameState : IGameStateBase
{
    public void Enter(GameManager gameManager) {
        Time.timeScale = 0;
    }
    public void Update(GameManager gameManager) {

    }
    public void Exit(GameManager gameManager) {
        Time.timeScale = 1;
    }
}
