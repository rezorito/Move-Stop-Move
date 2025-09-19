using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class SetShop : MonoBehaviour
{
    //public UIShopSkin uiShopSkin;
    //public GameObject itemPrefab;
    //public Transform contentParent;
    //public ItemDatabase itemDatabase;
    //public BtnSkinShopManager btnSkinShopManager;

    //public Transform skinPlayer;
    //public Transform parentHairPlayer1;
    //public Transform parentHairPlayer2;
    //public Transform pantPlayer;
    //public Transform parentSwingPlayer;
    //public Transform parentTailPlayer;

    //private List<GameObject> listBtns = new List<GameObject>();
    //private bool isPlayerSet;
    //public Material skinStart;
    //private GameObject btnStart = null;
    //private GameObject btnChooseItem = null;
    //private ItemBase chooseItem = null;

    //public TextMeshProUGUI effectItem;

    //public GameObject groupBuyItem;
    //public GameObject btnBuy;
    //public GameObject btnFreeOne;
    //public GameObject btnEquip;
    //public GameObject btnUnequip;

    //void GenerateShop()
    //{
    //    listBtns.Clear();
    //    foreach (Transform child in contentParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (var item in itemDatabase.allItems)
    //    {
    //        if (item.itemType != ItemType.Set) continue;
    //        GameObject newItem = Instantiate(itemPrefab, contentParent);

    //        //Chọn chosose btn ban đầu (nếu nhân vật có sẵn thì sẽ chọn vào btn đó và nếu k có sẵn cái nào thì tự chọn cái đầu tiên)
    //        if (isPlayerSet && btnStart == null)
    //        {
    //            chooseItem = item;
    //            btnStart = newItem;
    //        }
    //        else
    //        {
    //            if (item == chooseItem)
    //            {
    //                btnStart = newItem;
    //            }
    //        }
    //        listBtns.Add(newItem);

    //        if (DataManager.Ins.gameSave.list_SetIDOwn.Contains(item.id))
    //        {
    //            GameObject Lock = newItem.transform.Find("Lock").gameObject;
    //            Lock.SetActive(false);
    //        }

    //        Image iconImage = newItem.transform.Find("Icon").GetComponent<Image>();
    //        if(item.icon != null)
    //        {
    //            iconImage.sprite = item.icon;
    //        }

    //        Button buyButton = newItem.GetComponent<Button>();
    //        buyButton.onClick.AddListener(() =>
    //        {
    //            AudioManager.Ins.PlaySound_ButtonClick();
    //            btnChooseItem = newItem;
    //            chooseItem = item;
    //            UpdateUIBtn(newItem);
    //            DetailEquip(item);
    //            ActionWithItem(item);
    //        });
    //    }
    //}

    //private void UpdateUIBtn(GameObject itemBtn)
    //{
    //    foreach (GameObject btn in listBtns)
    //    {
    //        GameObject outBorder = btn.transform.Find("OutBorder").gameObject;
    //        if (btn == itemBtn)
    //        {
    //            outBorder.SetActive(true);
    //        }
    //        else
    //        {
    //            outBorder.SetActive(false);
    //        }
    //    }
    //}

    //private void DetailEquip(ItemBase itemBtn)
    //{
    //    effectItem.text = "+ " + itemBtn.atributes.valueString + " " + itemBtn.atributes.AtributeName;
    //    if (DataManager.Ins.gameSave.list_SetIDOwn.Contains(itemBtn.id))
    //    {
    //        groupBuyItem.SetActive(false);
    //        if(DataManager.Ins.gameSave.str_currentSetID == itemBtn.id) {
    //            btnEquip.SetActive(false);
    //            btnUnequip.SetActive(true);
    //        }
    //        else
    //        {
    //            btnEquip.SetActive(true);
    //            btnUnequip.SetActive(false);
    //        }
    //    } else
    //    {
    //        groupBuyItem.SetActive(true);
    //        GameObject textValuePrice = btnBuy.transform.Find("ValuePrice").gameObject;
    //        textValuePrice.GetComponent<TextMeshProUGUI>().text = itemBtn.price.ToString();

    //        btnEquip.SetActive(false);
    //        btnUnequip.SetActive(false);
    //    }
    //}

    //void ActionWithItem(ItemBase item)
    //{
    //    ResetOutfitDefault();
    //    if (item.skinMaterial != null)
    //    {
    //        if(skinPlayer != null)
    //        {
    //            Material[] mats = skinPlayer.gameObject.GetComponent<Renderer>().materials;
    //            for (int i = 0; i < mats.Length; i++)
    //            {
    //                mats[i] = item.skinMaterial;
    //            }
    //            skinPlayer.gameObject.GetComponent<Renderer>().materials = mats;
    //        }
    //        if (pantPlayer != null)
    //        {
    //            Material[] mats = pantPlayer.gameObject.GetComponent<Renderer>().materials;
    //            for (int i = 0; i < mats.Length; i++)
    //            {
    //                mats[i] = item.skinMaterial;
    //            }
    //            pantPlayer.gameObject.GetComponent<Renderer>().materials = mats;
    //        }
    //        else
    //        {
    //            Debug.Log("Chua co cho gan pant set");
    //        }
    //    }

    //    foreach (ItemBase it in item.subItems)
    //    {
    //        if(it.itemType == ItemType.Hair)
    //        {
    //            if (!it.highHair)
    //            {
    //                if (parentHairPlayer1 != null)
    //                {
    //                    GameObject hairPlayer = Instantiate(it.modelPrefab, parentHairPlayer1);
    //                }
    //                else
    //                {
    //                    Debug.Log("Chua co cho gan hair set");
    //                }
    //            } else
    //            {
    //                if (parentHairPlayer2 != null)
    //                {
    //                    GameObject hairPlayer = Instantiate(it.modelPrefab, parentHairPlayer2);
    //                }
    //                else
    //                {
    //                    Debug.Log("Chua co cho gan hair set");
    //                }
    //            }
    //        }
    //        if (it.itemType == ItemType.Wing)
    //        {
    //            if(parentSwingPlayer != null)
    //            {
    //                GameObject wingPlayer = Instantiate(it.modelPrefab, parentSwingPlayer);
    //            } else
    //            {
    //                Debug.Log("Chua co cho gan wing set");
    //            }
    //        }
    //        if (it.itemType == ItemType.Tail)
    //        {
    //            if (parentTailPlayer != null)
    //            {
    //                GameObject wingPlayer = Instantiate(it.modelPrefab, parentTailPlayer);
    //            }
    //            else
    //            {
    //                Debug.Log("Chua co cho gan tail set");
    //            }
    //        }
    //    }
    //}

    //private void SetupDetailOwnerItem()
    //{
    //    btnSkinShopManager.SetupButtons(
    //        chooseItem,
    //        () => {
    //            if(DataManager.Ins.gameSave.coin >= chooseItem.price) {
    //                AudioManager.Ins.PlaySound_ButtonClick();
    //                DataManager.Ins.SaveBuyItem(chooseItem.id, chooseItem.price, ItemType.Set);
    //                DetailEquip(chooseItem);
    //                GameObject Lock = btnChooseItem.transform.Find("Lock").gameObject;
    //                Lock.SetActive(false);
    //                UIControllerNormal.instance.uiMenuStart.LoadCoinOwn();
    //            }
    //        },
    //        () => {
    //            AudioManager.Ins.PlaySound_ButtonClick();
    //            DataManager.Ins.UpdateEquip(chooseItem.id, ItemType.Set);
    //            ActionWithItem(chooseItem);
    //            DetailEquip(chooseItem);
    //        },
    //        () => {
    //            AudioManager.Ins.PlaySound_ButtonClick();
    //            DataManager.Ins.UpdateEquip("", ItemType.Set);
    //            DetailEquip(chooseItem);
    //            ResetOutfitDefault();
    //        },
    //        () => {
    //            AudioManager.Ins.PlaySound_ButtonClick();
    //            Debug.Log("Chưa xong 🥲 ! Cho Free");
    //            DataManager.Ins.SaveBuyItem(chooseItem.id, 0, ItemType.Set);
    //            DetailEquip(chooseItem);
    //            GameObject Lock = btnChooseItem.transform.Find("Lock").gameObject;
    //            Lock.SetActive(false);
    //        }
    //    );
    //}

    //private void ResetOutfitDefault()
    //{
    //    Renderer rendererSkin = skinPlayer.GetComponent<Renderer>();
    //    Material[] skinMaterials = rendererSkin.materials;
    //    Renderer rendererPant = pantPlayer.GetComponent<Renderer>();
    //    Material[] pantMaterials = rendererSkin.materials;
    //    for (int i = 0; i < skinMaterials.Length; i++)
    //    {
    //        skinMaterials[i] = skinStart;
    //    }
    //    for (int i = 0; i < pantMaterials.Length; i++)
    //    {
    //        pantMaterials[i] = skinStart;
    //    }
    //    rendererSkin.materials = skinMaterials;
    //    rendererPant.materials = pantMaterials;

    //    foreach (Transform child in parentHairPlayer1)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    foreach (Transform child in parentHairPlayer2)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (Transform child in parentSwingPlayer)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (Transform child in parentTailPlayer)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}

    //private void OnEnable()
    //{
    //    isPlayerSet = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentSetID);
    //    if (!isPlayerSet)
    //    {
    //        chooseItem = itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentSetID);
    //    }
    //    ResetOutfitDefault();
    //    GenerateShop();
    //    StartCoroutine(SetupStartItem());
    //}
    //IEnumerator SetupStartItem()
    //{
    //    yield return null;
    //    if (btnStart != null)
    //    {
    //        btnChooseItem = btnStart;
    //        UpdateUIBtn(btnStart);
    //    }

    //    if (chooseItem != null)
    //    {
    //        DetailEquip(chooseItem);
    //        SetupDetailOwnerItem();
    //        ActionWithItem(chooseItem);
    //    }
    //}

    //private void OnDisable()
    //{
    //    chooseItem = null;
    //    btnStart = null;
    //    btnChooseItem = null;
    //    ResetOutfitDefault();
    //}
    public UIShopSkin uiShopSkin;
    public GameObject obj_itemPrefab;
    public Transform trams_contentParent;
    public Material mat_skinStart;

    private List<(Material mat_skin, Material mat_pant ,GameObject obj_hair1, GameObject obj_hair2, GameObject obj_shield, GameObject obj_wing, GameObject obj_tail , ItemPrefabs itemPrefabs, ItemBase item, bool isInit)> list_setIns 
        = new List<(Material, Material, GameObject, GameObject, GameObject, GameObject, GameObject, ItemPrefabs, ItemBase, bool)>();
    public ItemBase chooseItemPrevious = null;
    private bool isPlayerSet;
    public ItemPrefabs itemPrefabChooseItem = null;
    private ItemPrefabs itemPrefabPrevious = null;
    private ItemBase chooseItem = null;

    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if (!isInit) {
            isInit = true;
            GenerateShop();
        }
        SetupStartItem();
    }

    void GenerateShop() {
        foreach (ItemBase item in DataManager.Ins.list_SetData) {
            ItemPrefabs itemprefab = Instantiate(obj_itemPrefab, trams_contentParent).GetComponent<ItemPrefabs>();
            //Mở khóa các cái nhân vật đã có
            itemprefab.Init(item);
            list_setIns.Add((null, null, null, null, null, null, null, itemprefab, item, false));
            itemprefab.btn_selectItem.onClick.RemoveAllListeners();
            itemprefab.btn_selectItem.onClick.AddListener(() => {
                if (itemprefab == itemPrefabChooseItem) return;
                AudioManager.Ins.PlaySound_ButtonClick();
                chooseItem = item;
                ActionWithItem(item, itemprefab);
            });
        }

    }

    public void SetupStartItem() {
        isPlayerSet = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentSetID);
        if (!isPlayerSet) {
            chooseItem = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentSetID);
            uiShopSkin.chosseItem = chooseItem;
            var result = list_setIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
        else {
            chooseItem = DataManager.Ins.list_SetData[0];
            var result = list_setIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
    }
    public void Close() {
        gameObject.SetActive(false);
    }

    public void ActionWithItem(ItemBase itemBase, ItemPrefabs itemPrefabs) {
        ResetSet();
        itemPrefabChooseItem = itemPrefabs;
        uiShopSkin.chosseItem = itemBase;
        uiShopSkin.SetupGroupBuyBtn();
        UpdateUIChangeSelectedItem(itemPrefabs);
        int index = list_setIns.FindIndex(x => x.item == itemBase);
        if (index >= 0) {
            if (!list_setIns[index].isInit) {
                var insItem = InsItem(itemBase);
                list_setIns[index] = (insItem.mat_skin, insItem.mat_pant, insItem.obj_hair1, insItem.obj_hair2, insItem.obj_shield, insItem.obj_wing, insItem.obj_tail, itemPrefabs, itemBase, true);
            }
            InsSkin(list_setIns[index].mat_skin, list_setIns[index].mat_pant);
            if (list_setIns[index].obj_hair1 != null) list_setIns[index].obj_hair1.SetActive(true);
            if (list_setIns[index].obj_hair2 != null) list_setIns[index].obj_hair2.SetActive(true);
            if (list_setIns[index].obj_shield != null) list_setIns[index].obj_shield.SetActive(true);
            if (list_setIns[index].obj_wing != null) list_setIns[index].obj_wing.SetActive(true);
            if (list_setIns[index].obj_tail != null) list_setIns[index].obj_tail.SetActive(true);
        }
    }

    public void UpdateUIChangeSelectedItem(ItemPrefabs itemPrefabs) {
        if (itemPrefabPrevious == null) {
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        }
        else {
            itemPrefabPrevious.UIUnselectItem();
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        }
    }

    public (Material mat_skin, Material mat_pant, GameObject obj_hair1, GameObject obj_hair2, GameObject obj_shield, GameObject obj_wing, GameObject obj_tail) InsItem(ItemBase item) {
        if (uiShopSkin.player_Clone.trans_parentHairHigh1 == null || uiShopSkin.player_Clone.trans_parentHairHigh2 == null ||
            uiShopSkin.player_Clone.trans_parentShield == null || uiShopSkin.player_Clone.trans_parentSwing == null || uiShopSkin.player_Clone.trans_parentTail == null ) {
            Debug.Log("chưa gán vị trí trên đầu");
            return (null, null, null, null, null, null, null);
        }
        Material _mat_skin;
        if (item.skinMaterial != null) _mat_skin = item.skinMaterial;
        else _mat_skin = mat_skinStart;
        InsSkin(_mat_skin, _mat_skin);
        GameObject _obj_hair1 = null;
        GameObject _obj_hair2 = null;
        GameObject _obj_shield = null;
        GameObject _obj_wing = null;
        GameObject _obj_tail = null;
        foreach (ItemBase it in item.subItems) {
            if (it.itemType == ItemType.Hair) {
                if (!it.highHair) {
                    _obj_hair1 = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh1);
                }
                else {
                    _obj_hair2 = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh2);
                }
            } else if(it.itemType == ItemType.Shield) {
                _obj_shield = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentShield);
            }else if (it.itemType == ItemType.Wing) {
                _obj_wing = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentSwing);
            } else if (it.itemType == ItemType.Tail) {
                _obj_tail = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentTail);
            }
        }
        return (_mat_skin, _mat_skin, _obj_hair1, _obj_hair2, _obj_shield, _obj_wing, _obj_tail);
    }

    public void InsSkin(Material _mat_skin, Material _mat_pant) {
        Material[] skinMaterials = uiShopSkin.player_Clone.rend_skin.materials;
        for (int i = 0; i < skinMaterials.Length; i++) {
            skinMaterials[i] = _mat_skin;
        }
        uiShopSkin.player_Clone.rend_skin.materials = skinMaterials;
        Material[] pantMaterials = uiShopSkin.player_Clone.rend_pant.materials;
        for (int i = 0; i < pantMaterials.Length; i++) {
            pantMaterials[i] = _mat_pant;
        }
        uiShopSkin.player_Clone.rend_pant.materials = pantMaterials;
    }


    public void ResetSet() {
        if (list_setIns.Count != 0) {
            InsSkin(mat_skinStart, mat_skinStart);
            foreach (var item in list_setIns) {
                if (item.obj_hair1 != null) item.obj_hair1.SetActive(false);
                if (item.obj_hair2 != null) item.obj_hair2.SetActive(false);
                if (item.obj_shield != null) item.obj_shield.SetActive(false);
                if (item.obj_wing != null) item.obj_wing.SetActive(false);
                if (item.obj_tail != null) item.obj_tail.SetActive(false);
            }
        }
    }

    public void OnDisable() {
        chooseItem = null;
        if (itemPrefabPrevious != null) itemPrefabPrevious.UIUnselectItem();
        itemPrefabPrevious = null;
        itemPrefabChooseItem = null;
        ResetSet();
    }
}
