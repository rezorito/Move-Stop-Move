using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("----------------Common----------------")]
    [Header("------------Ref------------")]
    public GameObject obj_parent;
    public SystemGameplay systemGameplayParent;
    public RangeDetect rangeDetectParent;
    private Rigidbody rb;
    private Transform trans_Target;

    [Header("------------Ref------------")]
    public int int_bonusExp = 1;
    public int int_bonusCoin = 1;
    public bool isPiercing = false;
    private Vector3 vt3_startPosition;
    private Vector3 vt3_flyDirection;
    private float flt_speed = 0.2f;
    private float flt_rotateSpeed = -1500f;
    private bool isFlying = false;
    public bool isSpecialAttack = false;
    public bool isInit = false;
    public ItemBase itemWeapon = null;

    public void Init(GameObject _obj_parent, RangeDetect rangeDetect, SystemGameplay systemGameplay = null) {
        if (isInit) return;
        isInit = true;
        obj_parent = _obj_parent;
        rangeDetectParent = rangeDetect;
        systemGameplayParent = systemGameplay;
        gameObject.tag = "Weapon";
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 0;
        rb.angularDrag = 0;
        rb.useGravity = false;
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    public void FlyTo(Transform pointSpawnWeapon, Transform target, float angleOffset = 0f, bool _isSpecialAttack = false)
    {
        if (_isSpecialAttack) isSpecialAttack = _isSpecialAttack;
        trans_Target = target;
        gameObject.SetActive(true);
        transform.position = pointSpawnWeapon.position;
        vt3_startPosition = transform.position;

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;

        if(GameManager.instance.currentMode == GameMode.Normal)
        {
            if(systemGameplayParent != null)
            {
                float scaleWithPlayer = systemGameplayParent.int_levelSelf * 0.15f;
                transform.localScale = Vector3.one + new Vector3(scaleWithPlayer, scaleWithPlayer, scaleWithPlayer);
            }
        } else if(GameManager.instance.currentMode == GameMode.Zombie)
        {
            if (Player.instance.playerController.abilitySelected != null && Player.instance.playerController.abilitySelected.abilityID == "StartBiggerID") {
                float scaleWithAbility = Player.instance.playerController.flt_valueScaleBigger / 0.002f * 0.15f;
                transform.localScale = Vector3.one + new Vector3(scaleWithAbility, scaleWithAbility, scaleWithAbility);
            }
            else {
                transform.localScale = Vector3.one;
            }
        }
            //transform.localScale = Vector3.one;
        Quaternion rot = Quaternion.AngleAxis(angleOffset, Vector3.up);
        vt3_flyDirection = rot * dir;
        isFlying = true;
    }

    private void FixedUpdate()
    {
        if (!isFlying) return;
        if (isSpecialAttack) flt_speed = 0.35f;
        Vector3 newPos = transform.position + vt3_flyDirection * flt_speed * Time.deltaTime;
        rb.MovePosition(newPos);

        if (isSpecialAttack)
        {
            Quaternion targetRot = Quaternion.LookRotation(vt3_flyDirection);
            targetRot *= Quaternion.Euler(-90f, 0f, 0f);
            rb.MoveRotation(targetRot);
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            if (itemWeapon.attackRotation)
            {
                float yRot = flt_rotateSpeed * Time.time;
                Quaternion targetRot = Quaternion.Euler(-90f, yRot, 0f);
                rb.MoveRotation(targetRot);
            }
            else
            {
                Quaternion targetRot = Quaternion.LookRotation(vt3_flyDirection);
                targetRot *= Quaternion.Euler(-90f, 0f, 0f);
                rb.MoveRotation(targetRot);
            }
        }

        float distance = Vector3.Distance(vt3_startPosition, transform.position);
        if (isSpecialAttack)
        {
            if (distance >= rangeDetectParent.flt_detectionRadius * 2f)
            {
                DisableProjectile();
            }
        }
        else
        {
            if (distance >= rangeDetectParent.flt_detectionRadius)
            {
                DisableProjectile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject == obj_parent) return;
            if (obj_parent != null) {
                var systemGameplay = obj_parent.GetComponent<SystemGameplay>();
                if (systemGameplay != null) {
                    if (GameManager.instance.currentMode == GameMode.Normal) {
                        systemGameplayParent.AddExp(other.GetComponent<SystemGameplay>().int_levelSelf + 1);
                        systemGameplayParent.AddCoin(other.GetComponent<SystemGameplay>().int_levelSelf + 1);
                        if (!isSpecialAttack) DisableProjectile();
                    }
                    else if (GameManager.instance.currentMode == GameMode.Zombie) {
                        if (!isPiercing) DisableProjectile();
                    }
                }
                else {
                    Debug.LogWarning("Parent không có SystemGameplay: " + obj_parent.name);
                }
            }
            else {
                Debug.LogWarning("Weapon chưa được gán parent!");
            }
        }
    }

    private void DisableProjectile()
    {
        isFlying = false;
        if (isSpecialAttack && GameManager.instance.currentMode == GameMode.Normal) isSpecialAttack = false;
        gameObject.SetActive(false);
    }

    public void AwardKillZombieToParent(TypeZombie zombie)
    {
        systemGameplayParent.AddExp(zombie.valueScore * int_bonusExp);
        systemGameplayParent.AddCoin(1 * int_bonusCoin);
    }
}

