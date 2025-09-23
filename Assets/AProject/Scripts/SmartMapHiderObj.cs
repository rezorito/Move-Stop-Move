using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartMapHiderObj : MonoBehaviour {

    private GameObject obj_player;
    private Material mat_trigger;
    public List<GameObject> list_hideableObjectsAllDir = new List<GameObject>();
    public List<GameObject> list_hideableObjectsBotDir = new List<GameObject>();
    private Dictionary<GameObject, (bool isTrigger, Material _mat)> dict_objectStates = new Dictionary<GameObject, (bool, Material)>();

    public float flt_opacityBuidingHide = 0.1f;
    public float flt_hideRadius = 0.15f;
    public float flt_verticalHideDistance = 0.3f; // Khoảng cách theo trục Y
    public float flt_checkInterval = 0.1f; // Giảm CPU usage

    void Start() {
        obj_player = Player.instance.gameObject;
        flt_hideRadius = Player.instance.playerController.rangeDetect.flt_detectionRadius * 2;
        //flt_verticalHideDistance = Player.instance.playerController.rangeDetect.flt_detectionRadius + flt_hideRadius;
        SetupMaterialTrigger();
        InitializeMaterials();
        StartCoroutine(CheckObjectsCoroutine());
    }

    public void SetupMaterialTrigger() {
        mat_trigger = new Material(Shader.Find("Standard"));
        mat_trigger.SetFloat("_Mode", 3);
        mat_trigger.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat_trigger.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat_trigger.SetInt("_ZWrite", 0);
        mat_trigger.DisableKeyword("_ALPHATEST_ON");
        mat_trigger.EnableKeyword("_ALPHABLEND_ON");
        mat_trigger.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat_trigger.renderQueue = 3000;
        mat_trigger.SetColor("_Color", new Color(1f, 1f, 1f, flt_opacityBuidingHide));
    }

    void InitializeMaterials() {
        // Khởi tạo materials cho AllDir objects
        foreach (GameObject obj in list_hideableObjectsAllDir) {
            if (obj != null) {
                SetupObjectMaterial(obj);
            }
        }

        // Khởi tạo materials cho BotDir objects
        foreach (GameObject obj in list_hideableObjectsBotDir) {
            if (obj != null) {
                SetupObjectMaterial(obj);
            }
        }
    }

    void SetupObjectMaterial(GameObject obj) {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null) {
            // Tạo instance material để tránh ảnh hưởng đến material gốc
            Material originalMat = renderer.materials[0];
            dict_objectStates[obj] = (false, originalMat);
        }
    }

    IEnumerator CheckObjectsCoroutine() {
        while (true) {
            if (list_hideableObjectsAllDir.Count > 0)
                CheckObjectsAllDirVisibility();
            if (list_hideableObjectsBotDir.Count > 0)
                CheckObjectsBotDirVisibility();
            yield return new WaitForSeconds(flt_checkInterval);
        }
    }

    void CheckObjectsAllDirVisibility() {
        if (obj_player == null || list_hideableObjectsAllDir.Count == 0) return;

        Vector3 playerPos = obj_player.transform.position;

        foreach (GameObject obj in list_hideableObjectsAllDir) {
            if (obj == null) continue;

            Vector3 objPos = obj.transform.position;

            Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
            Vector2 objXZ = new Vector2(objPos.x, objPos.z);
            float horizontalDistance = Vector2.Distance(playerXZ, objXZ);

            bool shouldHide = horizontalDistance <= flt_hideRadius;

            // Chỉ thay đổi trạng thái khi cần
            if (dict_objectStates.ContainsKey(obj) && dict_objectStates[obj].isTrigger != shouldHide) {
                if(shouldHide) {
                    setMaterialInOutTrigger(obj, mat_trigger);
                } else {
                    setMaterialInOutTrigger(obj, dict_objectStates[obj]._mat);
                }
                dict_objectStates[obj] = (shouldHide, dict_objectStates[obj]._mat);
            }
        }
    }

    void CheckObjectsBotDirVisibility() {
        if (obj_player == null || list_hideableObjectsBotDir.Count == 0) return;

        Vector3 playerPos = obj_player.transform.position;

        foreach (GameObject obj in list_hideableObjectsBotDir) {
            if (obj == null) continue;

            Vector3 objPos = obj.transform.position;
            float verticalDistance = playerPos.z - objPos.z;

            bool shouldHide = verticalDistance >= flt_verticalHideDistance;
            Debug.Log(verticalDistance);
            // Chỉ thay đổi trạng thái khi cần
            if (dict_objectStates.ContainsKey(obj) && dict_objectStates[obj].isTrigger != shouldHide) {
                //setMaterialInOutTrigger(dict_objectStates[obj]._mat, shouldHide);
                //dict_objectStates[obj] = (shouldHide, dict_objectStates[obj]._mat);
                if (shouldHide) {
                    setMaterialInOutTrigger(obj, mat_trigger);
                }
                else {
                    setMaterialInOutTrigger(obj, dict_objectStates[obj]._mat);
                }
                dict_objectStates[obj] = (shouldHide, dict_objectStates[obj]._mat);
            }
        }
    }

    private void setMaterialInOutTrigger(GameObject obj, Material material = null) {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && renderer.materials.Length > 0) {
            Material[] materials = renderer.materials;
            materials[0] = material;
            renderer.materials = materials;
        }
    }

    //private void setMaterialInOutTrigger(Material material = null, bool isTrigger) {
    //    if(isTrigger) {
    //        material.SetFloat("_Mode", 3);
    //        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //        material.SetInt("_ZWrite", 0);
    //        material.DisableKeyword("_ALPHATEST_ON");
    //        material.EnableKeyword("_ALPHABLEND_ON");
    //        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //        material.renderQueue = 3000;
    //        material.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
    //    } else {
    //        material.SetFloat("_Mode", 0);
    //        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
    //        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
    //        material.SetInt("_ZWrite", 1);
    //        material.DisableKeyword("_ALPHATEST_ON");
    //        material.DisableKeyword("_ALPHABLEND_ON");
    //        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //        material.renderQueue = -1;
    //        material.SetColor("_Color", new Color(0f / 255f, 160f / 255f, 255f / 255f, 1f));
    //    }
    //}
}
