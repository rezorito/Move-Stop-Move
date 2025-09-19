using UnityEngine;

public class HomeGameState : IGameStateBase
{
    public void Enter(GameManager gameManager) {
         if (gameManager.currentMode == GameMode.Normal) {
            AudioManager.Ins.PlayMusic(SoundData.SoundName.Music_MainNormal);
            MapManager.instance.Init();
            SpawnManager.Instance.InitSpawn();
            UIControllerNormal.instance.OpenUIMenuStart();
        }
        else if (gameManager.currentMode == GameMode.Zombie){
            AudioManager.Ins.PlayMusic(SoundData.SoundName.Music_MainZombie);
            SpawnManagerZBCT.instance.InitSpawn();
            UIControllerZombie.instance.OpenUIMenuStartZombie();
        }
    }
    public void Update(GameManager gameManager) {

    }
    public void Exit(GameManager gameManager) {

    }
}
