using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { Weapon, Hair, Pant, Shield, Set, Tail, Wing }
public enum AtributeType { Range, MoveSpeed, AttackSpeed, Gold }


[System.Serializable]
public class SkinWeapon
{
    public string skinName;
    public Sprite spriteSkin;
    public bool isLock;
    public bool isCustom;
    public Material[] customMaterial;
    public Material skinMaterial;
}
[System.Serializable]
public class AtributeData
{
    public AtributeType atributeType;
    public string AtributeName;
    public string valueString;
    public float value;
}
[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemBase : ScriptableObject
{
    public string id;
    public string itemName;
    public ItemType itemType;
    public bool highHair;            //dành cho mũ (false : sát đầu; true : trên không trung)
    public GameObject modelPrefab;  //dành cho các cái model (weapon, hair, shield, tail, wing)
    public Material outfitMaterial; //dành cho quần
    public Sprite icon;
    public int price;               //wing, tail ko có (tại trong set)
    public Material[] materials;    //dành cho weapon
    public List<SkinWeapon> listSkinWeapon;
    public int skinWeaponChosse;
    public AtributeData atributes;

    public bool attackRotation;
    public List<ItemBase> subItems;     //Dành cho set
    public Material skinMaterial;       //Dành cho set  

}
