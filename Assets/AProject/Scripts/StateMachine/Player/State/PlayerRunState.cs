using UnityEngine;

public class PlayerRunState : IPlayerStateBase {
    public void Enter(Player player) {
        player.playerController.anim_Player.setBoolAnimIdle(false);
    }
    public void Update(Player player) {
        player.playerController.PlayerRunStateUpdate(player);
    }
    public void Exit(Player player) {

    }
}
