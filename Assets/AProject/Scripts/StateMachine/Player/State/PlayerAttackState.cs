using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAttackState : IPlayerStateBase {
    Player playerAttack;
    GameObject obj_weaponShow;
    Weapon weaponScript;
    public void Enter(Player player) {
        player.playerController.PlayerAttackStateEnter(player);
    }
    public void Update(Player player) {
        player.playerController.PlayerAttackStateUpdate(player);
    }
    public void Exit(Player player) {
        player.playerController.ResetAttack();
    }
}
