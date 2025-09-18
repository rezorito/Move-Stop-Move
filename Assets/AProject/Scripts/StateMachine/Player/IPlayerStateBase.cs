using UnityEngine;

public interface IPlayerStateBase
{
    void Enter(Player player);
    void Update(Player player);
    void Exit(Player player);
}
