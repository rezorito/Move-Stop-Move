using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AtributePlayerZombie;

public class AtributeItem : MonoBehaviour
{
    public TextMeshProUGUI effectText;
    public Button btnBuyAtribute;
    public Image imgBtn;
    public TextMeshProUGUI coinBuyText;

    private TypeAtriButeZombie typeAtriButeZombie;
    public void Init(TypeAtriButeZombie _typeAtriButeZombie, bool reset = false)
    {
        typeAtriButeZombie = _typeAtriButeZombie;
        if(reset)
        {
            resetAtribute();
        }
        setupEffectText();
        SetupBtnBuyAtribute_CanBuy();
        setupCoinBuy();
    }

    public void resetAtribute()
    {
        //atributeItem.level = 0;
        //atributeItem.valueBonusTotal = atributeItem.atributeBonus.valueDefault;
        //atributeItem.coinRequireTotal = atributeItem.atributeBonus.coinDefault;
    }

    public void setupActionBuyAtribute() {
        if(typeAtriButeZombie == TypeAtriButeZombie.Shield) {
            DataManager.Ins.UpdateShieldBonus();
        } else if (typeAtriButeZombie == TypeAtriButeZombie.Speed) {
            DataManager.Ins.UpdateSpeedBonus();
        } else if (typeAtriButeZombie == TypeAtriButeZombie.Range) {
            DataManager.Ins.UpdateRangeBonus();
        } else if (typeAtriButeZombie == TypeAtriButeZombie.MaxBullet) {
            DataManager.Ins.UpdateMaxBulletBonus();
        }
        setupCoinBuy();
        setupEffectText();
    }

    private void setupCoinBuy() {
        if (typeAtriButeZombie == TypeAtriButeZombie.Shield) {
            coinBuyText.text = DataManager.Ins.gameSave.int_coinAtriShield.ToString();
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Speed) {
            coinBuyText.text = DataManager.Ins.gameSave.int_coinAtriShield.ToString();
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Range) {
            coinBuyText.text = DataManager.Ins.gameSave.int_coinAtriRange.ToString();
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.MaxBullet) {
            coinBuyText.text = DataManager.Ins.gameSave.int_coinAtriMaxBullet.ToString();
        }
    }

    private void setupEffectText()
    {
        if (typeAtriButeZombie == TypeAtriButeZombie.Shield) {
            effectText.text = DataManager.Ins.gameSave.int_amountShieldZombie.ToString() + " Time";
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Speed) {
            effectText.text = "+ " + Mathf.RoundToInt(DataManager.Ins.gameSave.flt_speedBonusZombie * 100).ToString() + "% Speed";
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Range) {
            effectText.text = "+ " + Mathf.RoundToInt(DataManager.Ins.gameSave.flt_rangeBonusZombie * 100).ToString() + "% Range";
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.MaxBullet) {
            effectText.text = "Max: " + DataManager.Ins.gameSave.int_amountMaxBullet.ToString();
        }
    }

    public void SetupBtnBuyAtribute_CanBuy()
    {
        int coinRequireTotal = 0;
        if (typeAtriButeZombie == TypeAtriButeZombie.Shield) {
            coinRequireTotal = DataManager.Ins.gameSave.int_coinAtriShield;
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Speed) {
            coinRequireTotal = DataManager.Ins.gameSave.int_coinAtriShield;
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.Range) {
            coinRequireTotal = DataManager.Ins.gameSave.int_coinAtriRange;
        }
        else if (typeAtriButeZombie == TypeAtriButeZombie.MaxBullet) {
            coinRequireTotal = DataManager.Ins.gameSave.int_coinAtriMaxBullet;
        }
        if (DataManager.Ins.gameSave.coin < coinRequireTotal)
        {
            imgBtn.color = Color.white;
        }
        else
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#FF9E4A", out newColor))
            {
                imgBtn.color = newColor;
            }
        }
    }
}
