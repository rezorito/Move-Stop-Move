using UnityEngine;
public class LoadingGameState : IGameStateBase {
    public void Enter(GameManager gameManager) {
        AudioManager.Ins.PauseAllSound();
        DataManager.Ins.LoadData();
    }
    public void Update(GameManager gameManager) {

    }
    public void Exit(GameManager gameManager) {

    }
}
