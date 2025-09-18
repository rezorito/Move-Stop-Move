using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public Transform obj_MapParent; // nơi chứa các parent
    public GameObject obj_Map1Pref;
    public GameObject obj_Map2Pref;
    public Light lightMain;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void Init() {
        //Sinh map level hiện tại
        if(DataManager.Ins.gameSave.levelNormal == 1) {
            GameObject map1 = Instantiate(obj_Map1Pref, obj_MapParent, false);
            lightMain.intensity = 0.75f;
            map1.SetActive(true);
        } else {
            GameObject map2 = Instantiate(obj_Map2Pref, obj_MapParent, false);
            lightMain.intensity = 1f;
            map2.SetActive(true);
        }
    }
}
