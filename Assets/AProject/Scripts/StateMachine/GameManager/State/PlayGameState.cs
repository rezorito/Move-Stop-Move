using System.Collections;
using UnityEngine;
public class PlayGameState : IGameStateBase {
    public void Enter(GameManager gameManager) {
        AudioManager.Ins.PlayMusic(SoundData.SoundName.Music_BgrPlay);
        if (gameManager.currentMode == GameMode.Normal) {
            UIControllerNormal.instance.OpenUITut();
        }
        GameManager.instance.StartCoroutine(CheckWinGame());
        GameManager.instance.StartCoroutine(WaitForRevivePlayer());
    }
    public void Update(GameManager gameManager) {
        gameManager.playerTime += Time.deltaTime;
    }
    public void Exit(GameManager gameManager) {}

    public IEnumerator WaitForRevivePlayer() {
        while(Player.instance.playerController.CheckMaxReviveZombie()) {
            yield return null;
        }
        while (!Player.instance.IsPlayerStateRevive() && Player.instance.playerController.revivePlayer.isRevivePopup) {
            yield return null;
        }
        if (GameManager.instance.currentMode == GameMode.Normal && Player.instance.playerController.revivePlayer.isRevivePopup) UIControllerNormal.instance.OpenUIEnd(true);
        if (GameManager.instance.currentMode == GameMode.Zombie && Player.instance.playerController.revivePlayer.isRevivePopup) UIControllerZombie.instance.OpenUIEndZombie(true);
        while (!Player.instance.IsPlayerStateDie()) {
            yield return null;
        }
        GameManager.instance.StopCoroutine(CheckWinGame());
        GameManager.instance.ChangeStateEndGame();
    }

    public IEnumerator CheckWinGame() {
        if(GameManager.instance.currentMode == GameMode.Normal) {
            while (SpawnManager.Instance.getAmountEnemyRemaining() != 0) {
                yield return null;
            }
        } else if(GameManager.instance.currentMode == GameMode.Zombie) {
            while (SpawnManagerZBCT.instance.getAmountEnemyRemaining() != 0) {
                yield return null;
            }
        }
        GameManager.instance.StopCoroutine(WaitForRevivePlayer());
        Player.instance.ChangePlayerWinState();
        GameManager.instance.ChangeStateEndGame();

    }
}
