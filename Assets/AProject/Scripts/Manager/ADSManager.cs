using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSManager : MonoBehaviour
{
    public static ADSManager instance;
    public ApplovinMaxManager applovinMaxManager;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
