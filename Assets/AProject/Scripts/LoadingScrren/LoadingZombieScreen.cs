using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZombieScreen : MonoBehaviour
{
    public List<GameObject> list_imgChar;
    public List<GameObject> list_imgWeaponSpin;
    public string[] arrStr_content = new string[]
    {
        "Gold Supply is refreshing automatically.",
        "2.8m is the height of your character after start bigger.",
        "The bigger you are, the further you shoot.",
        "Move aside to dodge attack.",
        "Unlock weapons to be more powerful.",
        "Equip funny skins and show to your friends.",
        "Many skins and levels are upcoming."
    };

    public GameObject obj_char;
    public GameObject obj_weapon;
    public TextMeshProUGUI txt_content;
    private void OnEnable()
    {
        SetupScreen();
        StartCoroutine(LoadSceneZombie());
    }

    private void OnDisable()
    {
        foreach(GameObject a in list_imgChar)
        {
            a.SetActive(false);
        }
        foreach (GameObject a in list_imgWeaponSpin)
        {
            a.SetActive(false);
        }
    }

    private void Update()
    {
        obj_weapon.transform.Rotate(0f, 0f, -500f * Time.deltaTime);
    }

    private void SetupScreen()
    {
        GameObject objectCharRand = list_imgChar[Random.Range(0, list_imgChar.Count)];
        GameObject ObjectWeaponRand = list_imgWeaponSpin[Random.Range(0, list_imgWeaponSpin.Count)];
        string textContentRad = arrStr_content[Random.Range(0, arrStr_content.Length)];

        txt_content.text = textContentRad;
        objectCharRand.SetActive(true);
        ObjectWeaponRand.SetActive(true);
    }

    IEnumerator LoadSceneZombie()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        GameManager.instance.LoadScene("ZombieCity");
    }
}
