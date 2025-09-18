using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviveState : IPlayerStateBase {
    public void Enter(Player player) {
        player.playerController.PlayerReviveStateEnter(player);
    }
    public void Update(Player player) {
        player.playerController.PlayerReviveStateUpdate(player);
    }
    public void Exit(Player player) {

    }
}
