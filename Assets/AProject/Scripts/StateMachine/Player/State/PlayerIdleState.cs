using UnityEngine;

public class PlayerIdleState : IPlayerStateBase {
    public void Enter(Player player) {
        player.playerController.anim_Player.setBoolAnimIdle(true);
    }
    public void Update(Player player) {
        player.playerController.PlayerIdleStateUpdate(player);
    }

    public void Exit(Player player) {

    }
}
