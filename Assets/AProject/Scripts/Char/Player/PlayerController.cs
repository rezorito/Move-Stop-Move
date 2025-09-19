using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("----------------Common----------------")]
    [Header("-------------Refs-------------")]
    public InvisibleJoystick joystick;
    public RangeDetect rangeDetect;
    public AnimatorController anim_Player;
    public RevivePlayer revivePlayer;
    public SkinPlayer skinPlayer;
    public SystemGameplay systemGameplayPlayer;
    public Rigidbody rb_Player;
    public Collider col_Player;
    public Transform trans_pointWeaponAttack;
    public SoundController soundPlayerController;
    [Header("------------Variable------------")]
    public float flt_speed = 0.7f;
    public float flt_speedDefault;
    public float flt_rotateSpeed = 720f;
    public bool isSpecialAttack = false;
    public float flt_delayAttack = 0.7f;
    public float flt_timeToAttack = 0;
    public bool isAttack = true;
    public int rank = 0;
    public bool isMove = true;     //kiểm tra xem được di chuyển ko (khi attack)
    public string str_killerName = "";

    [Space]
    [Header("----------------Normal----------------")]
    [Header("-------------Refs-------------")]
    public CameraPos cameraMain;

    [Space]
    [Header("----------------Zombie----------------")]
    [Header("-------------Refs-------------")]
    public AtributePlayerZombie atributePlayerZombie;
    public GameObject obj_shield;
    public Transform trans_spawnWeaponBehind;
    public Transform trans_spawnWeaponLeft;
    public Transform trans_spawnWeaponRight;
    public Transform trans_spawnWeaponSpin;
    public AbilityBase abilitySelected = null;
    [Header("------------Variable------------")]
    public float flt_valueScaleBigger = 0.01f;
    public int int_shieldCount = 0;
    public float flt_durationShield = 1.5f;
    public bool isUseShield = false;
    public bool isPlayerReviveAbility = true;
    public int int_maxRevive = 0;
    public int int_bulletCount = 1;

    public void ActiveMove() {
        isMove = true;
    }
    public void ApplyAbility(AbilityBase ability) {
        abilitySelected = ability;
        if(ability.abilityType == AbilityType.AtributeAbility) {
            ability.ApplyAtributeAbility(this);
        } else if(ability.abilityType == AbilityType.AttackAbility) {
            if(abilitySelected.abilityID == "BladeCircleID") {
                abilitySelected.ApplyAttackAbility(this, null);
            }
            //Hmm dùng ở ActiveAttack 
        } else if(ability.abilityType == AbilityType.AttackEffectAbility) {
            skinPlayer.ApplyAbilityForListWeapon(ability);
        }
        else if(ability.abilityType == AbilityType.EconomyAbility) {
            skinPlayer.ApplyAbilityForListWeapon(ability);
        }
    }
    public void AddMaxReviveZombie(int i) {
        int_maxRevive += i;
    }
    public bool CheckMaxReviveZombie() {
        return int_maxRevive != 0;
    }
    public int GetBullet() {
        return int_bulletCount;
    }
    public void SetBulletBonus(int bonus) {
        int_bulletCount += bonus;
    }

    public void Awake() {
        flt_speed = flt_speedDefault;
    }

    private void Start() {
        if (GameManager.instance.currentMode == GameMode.Zombie) {
            setupSpeedZombie();
            setupShieldZombie();
        };
        skinPlayer.Init();
    }

    private void OnEnable() {
        skinPlayer.Init();
    }

    private void Update() {
        if (!GameManager.instance.IsGameStatePlay()) return;
        flt_timeToAttack += Time.deltaTime;
    }

    public void setupSpeedZombie() {
        flt_speed = flt_speedDefault + flt_speedDefault * DataManager.Ins.gameSave.flt_speedBonusZombie;
    }
    public void setupShieldZombie() {
        int_shieldCount = DataManager.Ins.gameSave.int_amountShieldZombie;
    }

    #region Player Idle State
    //Update Idle State
    public void PlayerIdleStateUpdate(Player player) {
        Vector2 input = joystick.Input;
        if (input.sqrMagnitude < 0.000001f && input.sqrMagnitude > -0.000001f) {
        }
        else {
            player.ChangePlayerRunState();
            return;
        }
        if (abilitySelected != null && abilitySelected.abilityID == "BladeCircleID") return;
        if (rangeDetect.GetEnemiesInRange().Count > 0) {
            if (flt_timeToAttack > flt_delayAttack || isAttack) {
                flt_timeToAttack = 0f;
                player.ChangePlayerAttackState();
            }
            return;
        }
    }
    #endregion

    #region Player Run State
    //Update Run State
    public void PlayerRunStateUpdate(Player player) {
        player.playerController.isAttack = true;
        Vector2 input = joystick.Input;
        if (input.sqrMagnitude < 0.000001f && input.sqrMagnitude > -0.000001f) {
            player.ChangePlayerIdleState();
            return;
        }
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;
        Vector3 move = moveDir * flt_speed * Time.fixedDeltaTime;
        rb_Player.MovePosition(rb_Player.position + move);
        Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
        rb_Player.rotation = Quaternion.Slerp(rb_Player.rotation, toRotation, flt_rotateSpeed * Time.fixedDeltaTime);
    }
    #endregion

    #region Player Attack State
    //Enter Attack State
    public void PlayerAttackStateEnter(Player player) {
        isAttack = false;
        isMove = false;
        SetupAttack();
    }

    //Update Attack State
    public void PlayerAttackStateUpdate(Player player) {
        if (isMove) {
            Vector2 input = joystick.Input;
            if (input.sqrMagnitude < 0.000001f && input.sqrMagnitude > -0.000001f) {
            }
            else {
                player.ChangePlayerIdleState();
                return;
            }
        }
    }

    //Setup khi tấn công
    private void SetupAttack() {
        LookAtEnemy();
        anim_Player.setBoolAnimAttack(true);
        skinPlayer.obj_weaponShow.SetActive(false);
    }

    //Xoay nhân vật đến mục tiêu
    private void LookAtEnemy() {
        Transform trans_target = rangeDetect.ChooseTarget();
        if (trans_target != null) {
            Vector3 direction = (trans_target.position - gameObject.transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero) {
                gameObject.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    //Thực hiện tấn công
    public void ActiveAttack() {
        Transform trans_target = rangeDetect.ChooseTarget();
        if (trans_target == null) {
            Player.instance.ChangePlayerIdleState();
            return;
        }
        soundPlayerController.PlaySound(SoundData.SoundName.Attack);
        if (GameManager.instance.currentMode == GameMode.Normal) {
            if (!skinPlayer.weaponAttackScript.isInit) skinPlayer.weaponAttackScript.Init(gameObject, rangeDetect, systemGameplayPlayer);
            skinPlayer.weaponAttackScript.FlyTo(trans_pointWeaponAttack, trans_target, 0f, isSpecialAttack);
            if (isSpecialAttack) SpecialAttackOff();
        } else if(GameManager.instance.currentMode == GameMode.Zombie) {
            if(abilitySelected == null || abilitySelected.abilityType != AbilityType.AttackAbility) {
                float angel = 0;
                for (int i=0; i<int_bulletCount; i++) {
                    Weapon weapon = skinPlayer.GetBullet();
                    if (!weapon.isInit) weapon.Init(gameObject, rangeDetect, systemGameplayPlayer);
                    weapon.FlyTo(trans_pointWeaponAttack, trans_target, angel, isSpecialAttack);
                    angel += Mathf.Pow(-1, i + 1) * 40f * (i + 1);
                }
            } else {
                abilitySelected.ApplyAttackAbility(this, trans_target);
            }
        }
    }

    //Reset lại tấn công
    public void ResetAttack() {
        skinPlayer.obj_weaponShow.SetActive(true);
        anim_Player.setBoolAnimAttack(false);
    }
    #endregion

    #region Player Revive State
    //Enter Revive State
    public void PlayerReviveStateEnter(Player player) {
        if (GameManager.instance.currentMode == GameMode.Normal) {
            setupPlayerDie();
            StartCoroutine(WaitForRevive(player));
        }
        else if (GameManager.instance.currentMode == GameMode.Zombie) {
            setupPlayerDie();
            if (CheckMaxReviveZombie()) {
                int_maxRevive--;
                StartCoroutine(PlayerRevive(player, 1f));
                return;
            }
            StartCoroutine(WaitForRevive(player));
        }
    }

    //Update Revive State
    public void PlayerReviveStateUpdate(Player player) {

    }

    public void Exit(Player player) {

    }

    public IEnumerator WaitForRevive(Player player) {
        // chờ đến khi 1 trong 2 flag được bật
        while (!revivePlayer.isUseRevivePopup && !revivePlayer.isUnuseRevivePopup) {
            yield return null; // nhường sang frame tiếp theo
        }

        if (revivePlayer.isUseRevivePopup) {
            yield return StartCoroutine(PlayerRevive(player));
        }
        if (revivePlayer.isUnuseRevivePopup) {
            player.ChangePlayerDieState();
        }
    }

    IEnumerator PlayerRevive(Player player, float time = 0f) {
        yield return new WaitForSeconds(time);
        setUpPlayerAlive(player);
        revivePlayer.Revive();
    }
    IEnumerator useShieldZombie() {
        isUseShield = true;
        obj_shield.SetActive(true);
        yield return new WaitForSeconds(flt_durationShield);
        int_shieldCount--;
        obj_shield.SetActive(false);
        isUseShield = false;
    }
    public void setupPlayerDie() {
        col_Player.enabled = false;
        anim_Player.setBoolAnimDead(true);
    }
    public void setUpPlayerAlive(Player player) {
        col_Player.enabled = true;
        anim_Player.setBoolAnimDead(false);
        player.ChangePlayerIdleState();
    }   
    #endregion

    #region Player Die State

    #endregion

    private void OnTriggerEnter(Collider other) {
        if (Player.instance.IsPlayerStateWin()) return;
        if (GameManager.instance.currentMode == GameMode.Normal) {
            if (other.CompareTag("Weapon")) {
                if (other.gameObject == skinPlayer.obj_weaponAttack) return;
                soundPlayerController.PlaySound(SoundData.SoundName.WeaponHit);
                if (SpawnManager.Instance != null) rank = SpawnManager.Instance.getAmountEnemyRemaining() + 1;
                str_killerName = other.GetComponent<Weapon>().obj_parent.GetComponent<EnemyAI>().str_nameSelf;
                if(!revivePlayer.isRevivePopup) {
                    setupPlayerDie();
                    Player.instance.ChangePlayerDieState();
                    return;
                }
                Player.instance.ChangePlayerReviveState();
            }
        }
        else if (GameManager.instance.currentMode == GameMode.Zombie) {
            if (other.CompareTag("Enemy")) {
                if (isUseShield) return;
                if (int_shieldCount > 0 && !isUseShield) {
                    StartCoroutine(useShieldZombie());
                    return;
                }
                if (CheckMaxReviveZombie() || revivePlayer.isRevivePopup) {
                    Player.instance.ChangePlayerReviveState();
                    return;
                }
                if (!revivePlayer.isRevivePopup) {
                    Player.instance.ChangePlayerDieState();
                    return;
                }
            }
        }
        if (other.CompareTag("Gift")) {
            if(!isSpecialAttack) SpecialAttackOn();
        }
    }

    public void SpecialAttackOn() {
        isSpecialAttack = true;
        rangeDetect.setOnSpecialAttack();
        cameraMain.setOnSpecialAttack();
    }

    public void SpecialAttackOff() {
        isSpecialAttack = false;
        rangeDetect.setOffSpecialAttack();
        cameraMain.setOffSpecialAttack();
    }
}
