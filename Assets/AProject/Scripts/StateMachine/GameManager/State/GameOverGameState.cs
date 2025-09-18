using UnityEngine;
public class GameEndState : IGameStateBase {
    public void Enter(GameManager gameManager) {
        AudioManager.Ins.PauseAllMusic();
        AudioManager.Ins.PauseAllSound();
        if (GameManager.instance.currentMode == GameMode.Normal) UIControllerNormal.instance.OpenUIEnd(false);
        if (GameManager.instance.currentMode == GameMode.Zombie) UIControllerZombie.instance.OpenUIEndZombie(false);
        if(Player.instance.IsPlayerStateWin()) {
            AudioManager.Ins.PlaySound_ResultGame(true);
            if (GameManager.instance.currentMode == GameMode.Normal) DataManager.Ins.SaveLevelNormal();
            if (GameManager.instance.currentMode == GameMode.Zombie) DataManager.Ins.SaveLevelZombie();
        } else {
            AudioManager.Ins.PlaySound_ResultGame(false);
        }
    }
    public void Update(GameManager gameManager) {

    }

    public void Exit(GameManager gameManager) {

    }
}
