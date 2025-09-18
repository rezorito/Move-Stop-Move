using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AtributePlayerZombie;

public class UIMenuStartZombie : MonoBehaviour {
    [Header("----------------PanelRandomAbility----------------")]
    [Header("------------Ref------------")]
    public AbilityDatabase abilityDatabase;
    public TextMeshProUGUI txt_countDaySurvival;
    public TextMeshProUGUI txt_amountEnemyRemain;
    public TextMeshProUGUI txt_coinOwn;
    public TextMeshProUGUI txt_abilityName;
    public Image img_ability;
    public Button btn_goHome;   //Về Scene Normal
    public Button btn_randomAbility;
    public Button btn_PlayWhithoutAbility;
    public Button btn_PlayWithAbilityADS;

    private AbilityBase[] allAbilitys;
    private AbilityBase selectAbility;
    [Header("------------Variable------------")]
    //public PlayerAttack playerAttack;
    //public PlayerMovement playerMovement;
    public bool isInitRandomAbility = false;
    [Header("----------------PanelRandomAbility----------------")]
    [Header("------------Ref------------")]
    public AtributePlayerZombie atributePlayerZombie;
    public PlayerController playerController;

    public AtributeItem AtriItemShield;
    public AtributeItem AtriItemSpeed;
    public AtributeItem AtriItemRange;
    public AtributeItem AtriItemBullet;
    [Header("------------Variable------------")]
    public bool isInitGrowthAtribute = false;
    public bool resetAtribute = false;

    void Start() {
        playerController = Player.instance.playerController;
        //Init();
    }

    public void Init() {
        gameObject.SetActive(true);
        txt_countDaySurvival.text = "Day " + DataManager.Ins.gameSave.levelZombie;
        txt_amountEnemyRemain.text = SpawnManagerZBCT.instance.getAmountEnemyRemaining().ToString();
        LoadCoinOwn();
        InitPanelRandomAbility();
        InitPanelGrowthAtribute();
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void InitPanelRandomAbility() {
        if (!isInitRandomAbility) {
            isInitRandomAbility = true;
            allAbilitys = abilityDatabase.allAbilitys;
            SetupBtnPanelRandomAbility();
        }
        RandomAbility();
    }

    public void InitPanelGrowthAtribute() {
        if (!isInitGrowthAtribute) {
            isInitGrowthAtribute = true;
        }
        SetupGrowthAtribute();
    }

    public void SetupBtnPanelRandomAbility() {
        btn_goHome.onClick.RemoveAllListeners();
        btn_randomAbility.onClick.RemoveAllListeners();
        btn_PlayWhithoutAbility.onClick.RemoveAllListeners();
        btn_PlayWithAbilityADS.onClick.RemoveAllListeners();

        btn_goHome.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.LoadScene("MainScene");
        });
        btn_randomAbility.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            RandomAbility();
        });
        btn_PlayWhithoutAbility.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            UIControllerZombie.instance.OpenUITut();
            UIControllerZombie.instance.OpenUIInPlayZombie();
            Close();
        });
        btn_PlayWithAbilityADS.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            Player.instance.playerController.ApplyAbility(selectAbility);
            UIControllerZombie.instance.OpenUITut();
            UIControllerZombie.instance.OpenUIInPlayZombie();
            Close();
        });
    }

    public void RandomAbility() {
        AbilityBase randAbi = allAbilitys[Random.Range(0, allAbilitys.Length)];
        selectAbility = randAbi;
        img_ability.sprite = randAbi.abilitySprite;
        txt_abilityName.text = randAbi.abilityName;
    }

    public void SetAbility() {
        if (selectAbility.abilityType == AbilityType.AttackAbility) {
            //playerAttack.setAttackAbility(selectAbility);
        }
        else if (selectAbility.abilityType == AbilityType.AttackEffectAbility) {
            //playerAttack.setAttackEffectAbility(selectAbility);
        }
        else if (selectAbility.abilityType == AbilityType.AtributeAbility) {
            //playerMovement.setAtributeAbility(selectAbility);
        }
        else if (selectAbility.abilityType == AbilityType.EconomyAbility) {
            //playerAttack.setEconomyAbility(selectAbility);
        }
    }

    public void LoadCoinOwn() {
        txt_coinOwn.text = DataManager.Ins.gameSave.coin.ToString();
    }

    public void SetupGrowthAtribute() {
        AtriItemShield.Init(TypeAtriButeZombie.Shield, resetAtribute);
        SetupActionBtnUpAtribute(AtriItemShield, DataManager.Ins.gameSave.int_coinAtriShield);
        AtriItemSpeed.Init(TypeAtriButeZombie.Speed, resetAtribute);
        SetupActionBtnUpAtribute(AtriItemSpeed, DataManager.Ins.gameSave.int_coinAtriSpeed);
        AtriItemRange.Init(TypeAtriButeZombie.Range, resetAtribute);
        SetupActionBtnUpAtribute(AtriItemRange, DataManager.Ins.gameSave.int_coinAtriRange);
        AtriItemBullet.Init(TypeAtriButeZombie.MaxBullet, resetAtribute);
        SetupActionBtnUpAtribute(AtriItemBullet, DataManager.Ins.gameSave.int_coinAtriMaxBullet);
    }

    public void SetupActionBtnUpAtribute(AtributeItem atributeItem, int coinRequireTotal) {
        atributeItem.btnBuyAtribute.onClick.RemoveAllListeners();
        atributeItem.btnBuyAtribute.onClick.AddListener(() => {
            if (DataManager.Ins.gameSave.coin >= coinRequireTotal) {
                AudioManager.Ins.PlaySound_ButtonClick();
                atributeItem.setupActionBuyAtribute();  //mua atribute của nó
                //reset lại nút các atribute khác
                AtriItemShield.SetupBtnBuyAtribute_CanBuy();
                AtriItemSpeed.SetupBtnBuyAtribute_CanBuy();
                AtriItemRange.SetupBtnBuyAtribute_CanBuy();
                AtriItemBullet.SetupBtnBuyAtribute_CanBuy();
                //hiển thị lại tiền
                LoadCoinOwn();

                //Setup lại atribute
                playerController.rangeDetect.setupRangeDetectZombie();
                playerController.setupSpeedZombie();
                playerController.setupShieldZombie();
            } else {
                Debug.Log("Tiền đâu?");
            }
        });
    }
}
