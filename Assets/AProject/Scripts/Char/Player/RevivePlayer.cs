using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    public static RevivePlayer instance;

    [Header("------------Ref------------")]
    public Transform trans_Player;
    [Header("------------Variable------------")]
    public bool isRevivePopup = true;   //kiểm tra còn revive của popup k
    public bool isUseRevivePopup = false; //kiểm tra xem sử dụng revive popup?
    public bool isUnuseRevivePopup = false; //kiểm tra xem ko sử dụng revive popup?

    void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }

    public void Revive()
    {
        if(GameManager.instance.currentMode == GameMode.Normal)
        {
            //Transform randTrans = arr_pointReviveNormal[Random.Range(0, arr_pointReviveNormal.Length)];
            //trans_Player.position = new Vector3(randTrans.position.x, trans_Player.position.y, randTrans.position.z);
            trans_Player.position = GameConstants.vt3_spawnPoinNorMal;
        } else if(GameManager.instance.currentMode == GameMode.Zombie)
        {
            //Transform randTrans = arr_pointReviveZombie[Random.Range(0, arr_pointReviveZombie.Length)];
            //trans_Player.position = new Vector3(randTrans.position.x, trans_Player.position.y, randTrans.position.z);
            trans_Player.position = GameConstants.vt3_spawnPoinZombie;
        }
    }
}
