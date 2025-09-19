using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutController : MonoBehaviour
{
    public GameObject obj_wallForward;
    public GameObject obj_stopText;
    public GameObject obj_arrowImg;
    public GameObject obj_moveText;
    public GameObject obj_poolEnemy;
    public List<GameObject> list_enemyTut = new List<GameObject>();
    public Animator anim_panelGoDark;

    public int int_amountEnmyTut = 0;

    private void Awake() {
        int_amountEnmyTut = list_enemyTut.Count;
    }

    public void EnemyTutDie() {
        int_amountEnmyTut--;
        if(int_amountEnmyTut == 0) {
            obj_wallForward.SetActive(false);
            obj_stopText.SetActive(false);
            obj_arrowImg.SetActive(true);
            obj_moveText.SetActive(true);
        }
    }

    public void SetupStartTut() {
        obj_poolEnemy.SetActive(true);
        obj_stopText.SetActive(true);
    }

    public void SetupEndTut() {
        DataManager.Ins.gameSave.isDoneTutGameplay = true;
        StartCoroutine(LoadSceneNext());
    }

    IEnumerator LoadSceneNext() {
        anim_panelGoDark.SetTrigger("GoDark");
        InvisibleJoystick.instance.TurnOffShowJoystick();
        yield return new WaitForSeconds(1f);
        GameManager.instance.ReLoadScene();
    }
}
