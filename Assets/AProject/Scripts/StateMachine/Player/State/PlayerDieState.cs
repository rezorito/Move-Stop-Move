using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : IPlayerStateBase {
    public void Enter(Player player) {
        player.playerController.anim_Player.setBoolAnimDead(true);
        Debug.Log("Player die.");
    }
    public void Update(Player player) {
    }
    public void Exit(Player player) {
    }
}
