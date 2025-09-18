using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SystemGameplay : MonoBehaviour
{
    [Header("------------Ref------------")]
    public LevelCharNormal levelCharNormal;
    public LevelCharZombie levelCharZombie;
    [Header("------------Variable------------")]
    public int int_levelSelf = 0;
    public int int_scoreSelf = 0;
    public int int_coinReceive = 0;
    public float flt_GrowthLevelUp = 0.002f;
    public bool isPlayer = false;

    public void setScoreSelf(int _scoreSelf)
    {
        int_scoreSelf = _scoreSelf;
    }
    public int getScoreSelf()
    {
        return int_scoreSelf;
    }
    public int getCoinReceive()
    {
        return int_coinReceive;
    }

    public void AddExp(int levelEnemy)
    {
        int_scoreSelf += levelEnemy;
        CheckLevel();
    }

    public void AddCoin(int _coinReceive)
    {
        int_coinReceive += _coinReceive;
    }

    private void CheckLevel()
    {
        if(GameManager.instance.currentMode == GameMode.Normal)
        {
            if(int_levelSelf < levelCharNormal.allLevels.Length && int_scoreSelf >= levelCharNormal.allLevels[int_levelSelf].expRequire)
            {
                if (isPlayer) AudioManager.Ins.PlaySound_LevelUp();
                int_levelSelf += 1;
                UpdateUILevel();
            }
        } else if(GameManager.instance.currentMode == GameMode.Zombie)
        {
            if (int_levelSelf < levelCharZombie.allLevels.Length && int_scoreSelf >= levelCharZombie.allLevels[int_levelSelf].expRequire) {
                int_levelSelf += 1;
                if (Player.instance.playerController.GetBullet() < DataManager.Ins.gameSave.int_amountMaxBullet) {
                    if (isPlayer) AudioManager.Ins.PlaySound_LevelUp();
                    Player.instance.playerController.SetBulletBonus(1);
                }
            }
        }
    }

    public void UpdateUILevel()
    {
        Vector3 transformLevelUp = new Vector3(flt_GrowthLevelUp, flt_GrowthLevelUp, flt_GrowthLevelUp);
        transform.localScale += transformLevelUp;
    }
}
