using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateBase
{
    void Enter(GameManager gameManager);
    void Update(GameManager gameManager);
    void Exit(GameManager gameManager);
}
