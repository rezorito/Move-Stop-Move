using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;
    public IPlayerStateBase currentPlayerState;

    public PlayerController playerController;

    public bool IsPlayerStateIdle() => currentPlayerState is PlayerIdleState;
    public bool IsPlayerStateRun() => currentPlayerState is PlayerRunState;
    public bool IsPlayerStateAttack() => currentPlayerState is PlayerAttackState;
    public bool IsPlayerStateRevive() => currentPlayerState is PlayerReviveState;
    public bool IsPlayerStateDie() => currentPlayerState is PlayerDieState;
    public bool IsPlayerStateWin() => currentPlayerState is PlayerWinState;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        ChangePlayerIdleState();
    }


    void Update() {
        if (!GameManager.instance.IsGameStatePlay() || IsPlayerStateRevive() || IsPlayerStateDie() || IsPlayerStateWin()) return;
        currentPlayerState?.Update(this);
    }

    public void ChangePlayerState(IPlayerStateBase playerStateBase) {
        currentPlayerState?.Exit(this);
        currentPlayerState = playerStateBase;
        currentPlayerState.Enter(this);
    }

    public void ChangePlayerIdleState() {
        ChangePlayerState(new PlayerIdleState());
    }

    public void ChangePlayerRunState() {
        ChangePlayerState(new PlayerRunState());
    }

    public void ChangePlayerAttackState() {
        ChangePlayerState(new PlayerAttackState());
    }

    public void ChangePlayerReviveState() {
        ChangePlayerState(new PlayerReviveState());
    }

    public void ChangePlayerDieState() {
        ChangePlayerState(new PlayerDieState());
    }

    public void ChangePlayerWinState() {
        ChangePlayerState(new PlayerWinState());
    }
}
