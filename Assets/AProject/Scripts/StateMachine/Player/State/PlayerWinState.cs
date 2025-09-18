using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinState : IPlayerStateBase {
    public void Enter(Player player) {
        player.playerController.anim_Player.setBoolAnimWin(true);
    }
    public void Update(Player player) {

    }
    public void Exit(Player player) {
        player.playerController.anim_Player.setBoolAnimWin(false);
    }
}
