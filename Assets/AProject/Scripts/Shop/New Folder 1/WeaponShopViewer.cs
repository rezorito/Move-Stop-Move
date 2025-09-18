using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopViewer : MonoBehaviour
{
    [Header("Wiring")]
    public Transform stageRoot;
    public Camera shopCamera;              // camera preview
    public RawImage targetImage;           // RawImage trong UI
    public RenderTexture renderTexture;    // RT_WeaponPreview

    [Header("View")]
    public bool autoSpin = true;
    public float spinSpeed = 30f;          // độ quay preview
    public float flt_zoomFactor = 1f;
    public Vector3 modelEulerOffset = new Vector3(180, 180, -45); // tùy model
    private Vector3 scaleModelSkin = new Vector3(1f, 1f, 1f);
    private Vector3 scaleModelPrefab = new Vector3(4f, 4f, 4f);

    [Header("Layer")]
    public string previewLayerName = "WeaponPreview";

    private GameObject currentWeapon;
    private readonly Dictionary<string, List<MatSlot>> parts = new(); // key = "Handle", "Blade", ...

    private class MatSlot
    {
        public Renderer rend;
        public int matIndex;
        public Material instancedMat;
    }

    void Awake()
    {
        if (shopCamera && renderTexture) shopCamera.targetTexture = renderTexture;
        if (targetImage && renderTexture) targetImage.texture = renderTexture;
    }

    void Update()
    {
        //if (autoSpin && currentWeapon)
        //{
        //    currentWeapon.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        //}
    }

    // Public API: gọi để hiển thị một prefab vũ khí mới
    public void ShowWeapon(ItemBase item)
    {
        ClearCurrent();

        if (!item || !stageRoot) return;

        currentWeapon = Instantiate(item.modelPrefab, stageRoot);
        if(DataManager.Ins.gameSave.list_WeaponIDOwn.Contains(item.id)) {
            if(DataManager.Ins.gameSave.str_currentWeaponID == item.id) {
                if (item.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].isCustom) {
                    SetupMaterialCustom(item.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].customMaterial);
                } else {
                    SetupMaterial(item.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].skinMaterial);
                }
            }
        }
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(modelEulerOffset);
        currentWeapon.transform.localScale = scaleModelPrefab;

        // set layer cho toàn bộ cây
        int layer = LayerMask.NameToLayer(previewLayerName);
        SetLayerRecursively(currentWeapon, layer);

        // tạo instance material + lập map các phần
        BuildMaterialMap(currentWeapon);

        // đặt camera nhìn vừa khung
        FitCameraToObjectBounds();
    }

    public void ShowWeaponSkin(GameObject weaponPrefab, Transform pointSpawn)
    {
        if (!weaponPrefab || !pointSpawn) return;

        currentWeapon = Instantiate(weaponPrefab, pointSpawn);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(modelEulerOffset);
        currentWeapon.transform.localScale = scaleModelSkin;

        // set layer cho toàn bộ cây
        int layer = LayerMask.NameToLayer(previewLayerName);
        SetLayerRecursively(currentWeapon, layer);

        // tạo instance material + lập map các phần
        BuildMaterialMap(currentWeapon);

        // đặt camera nhìn vừa khung
        FitCameraToObjectBounds();
    }

    public void ClearCurrent()
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
        parts.Clear();
    }

    // Đổi màu theo key phần (ví dụ "Handle", "Blade", "Head", ...)

    public void SetupMaterial(Material _material)
    {
        Material[] weaponMaterials = currentWeapon.GetComponent<Renderer>().materials;
        for(int i=0; i<weaponMaterials.Length; i++)
        {
            weaponMaterials[i] = _material;
        }
        currentWeapon.GetComponent<Renderer>().materials = weaponMaterials;
    }

    public void SetupMaterialCustom(Material[] _material)
    {
        Material[] weaponMaterials = currentWeapon.GetComponent<Renderer>().materials;
        for (int i = 0; i < weaponMaterials.Length; i++)
        {
            weaponMaterials[i] = _material[i];
        }
        currentWeapon.GetComponent<Renderer>().materials = weaponMaterials;
    }

    public void SetPartColor(string partKey, Color color)
    {
        if (!parts.TryGetValue(partKey, out var slots)) return;

        foreach (var slot in slots)
        {
            var mat = slot.instancedMat;
            string cp = GetColorProperty(mat);
            if (!string.IsNullOrEmpty(cp))
            {
                mat.SetColor(cp, color);
            }
        }
    }

    public void SetMaterialColor(int materialIndex, Color color)
    {
        if (!currentWeapon) return;
        var rends = currentWeapon.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in rends)
        {
            if (rend.materials.Length > materialIndex)
            {
                var mat = rend.materials[materialIndex];
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", color);
                else if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", color);
            }
        }
    }

    // Nếu muốn set theo tên material gốc hoặc tên object
    public void SetByRendererNameContains(string nameContains, Color color)
    {
        foreach (var kv in parts)
        {
            foreach (var slot in kv.Value)
            {
                if (slot.rend && slot.rend.name.ToLower().Contains(nameContains.ToLower()))
                {
                    string cp = GetColorProperty(slot.instancedMat);
                    if (!string.IsNullOrEmpty(cp))
                        slot.instancedMat.SetColor(cp, color);
                }
            }
        }
    }

    // ---------- Helpers ----------

    private void BuildMaterialMap(GameObject root)
    {
        parts.Clear();
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            var mats = rend.sharedMaterials;
            if (mats == null) continue;

            var newMats = new Material[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] == null) continue;

                // Instance material để tùy biến
                var inst = new Material(mats[i]);
                newMats[i] = inst;

                // Tìm key phần dựa vào tên object (hoặc WeaponPartTag)
                string key = GuessPartKey(rend.gameObject);

                if (!parts.ContainsKey(key)) parts[key] = new List<MatSlot>();
                parts[key].Add(new MatSlot
                {
                    rend = rend,
                    matIndex = i,
                    instancedMat = inst
                });
            }

            // Gán lại materials đã instance
            rend.materials = newMats;
        }
    }

    private string GuessPartKey(GameObject go)
    {
        // Ưu tiên component tag nếu có
        var tagComp = go.GetComponentInParent<WeaponPartTag>(true);
        if (tagComp && !string.IsNullOrWhiteSpace(tagComp.key))
            return tagComp.key;

        // fallback: dựa vào tên object
        string n = go.name.ToLower();
        if (n.Contains("handle") || n.Contains("grip") || n.Contains("hilt") || n.Contains("can") || n.Contains("cán"))
            return "Handle";
        if (n.Contains("blade") || n.Contains("edge") || n.Contains("luoi") || n.Contains("lưỡi"))
            return "Blade";
        if (n.Contains("head") || n.Contains("tip") || n.Contains("dau") || n.Contains("đầu"))
            return "Head";
        if (n.Contains("guard") || n.Contains("pommel"))
            return "Guard";

        return "Other";
    }

    private void FitCameraToObjectBounds()
    {
        if (!shopCamera || !currentWeapon) return;

        Bounds b = GetHierarchyBounds(currentWeapon);
        Vector3 center = b.center;

        shopCamera.transform.position = center + new Vector3(0, 0, -1f);
        shopCamera.transform.LookAt(center);

        float radius = b.extents.magnitude;
        float fovRad = shopCamera.fieldOfView * Mathf.Deg2Rad;
        float dist = radius / Mathf.Sin(fovRad * 0.5f);

        shopCamera.transform.position = center - shopCamera.transform.forward * dist * flt_zoomFactor;

        shopCamera.nearClipPlane = Mathf.Max(0.01f, dist - radius * 2f);
        shopCamera.farClipPlane = dist + radius * 4f;
    }

    private Bounds GetHierarchyBounds(GameObject root)
    {
        var rends = root.GetComponentsInChildren<Renderer>(true);
        Bounds b = new Bounds(root.transform.position, Vector3.zero);
        bool inited = false;

        foreach (var r in rends)
        {
            if (!inited)
            {
                b = r.bounds;
                inited = true;
            }
            else b.Encapsulate(r.bounds);
        }
        if (!inited) b = new Bounds(root.transform.position, Vector3.one * 0.5f);
        return b;
    }

    private void SetLayerRecursively(GameObject go, int layer)
    {
        if (layer >= 0) go.layer = layer;
        foreach (Transform c in go.transform)
            SetLayerRecursively(c.gameObject, layer);
    }

    private string GetColorProperty(Material m)
    {
        // URP Lit
        if (m.HasProperty("_BaseColor")) return "_BaseColor";
        // Built-in Standard
        if (m.HasProperty("_Color")) return "_Color";
        // HDRP Lit (thường cũng _BaseColor)
        if (m.HasProperty("_BaseColor")) return "_BaseColor";
        // Nếu shader custom, bạn đổi theo property của nó
        return null;
    }
}
