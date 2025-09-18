using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiftManager : MonoBehaviour
{
    [Header("------------Ref------------")]
    public GameObject obj_giftPrefab;
    public List<GameObject> list_Gift = new List<GameObject>();

    [Header("------------Variable------------")]
    public float flt_timeDelayMin = 5f;
    public float flt_timeDelayMax = 15f;
    public int int_maxAmountInsStart = 3;

    private void Start()
    {
        list_Gift.Clear();
        SetupSpawnGift();
        StartCoroutine(SpawnGift());
    }

    public void SetupSpawnGift() {
        for(int i = 0; i < int_maxAmountInsStart; i++) {
            AddGift();
        }
    }

    IEnumerator SpawnGift()
    {
        while(true)
        {
            if (!GameManager.instance.IsGameStatePlay())
            {
                yield return null;
                continue;
            }
            float time = Random.Range(flt_timeDelayMin, flt_timeDelayMax);
            yield return new WaitForSeconds(time);
            GameObject obj_gift = GetGiftOnDisable();
            obj_gift.transform.localPosition = GetRandomSpawnPosition();
            obj_gift.SetActive(true);
            yield return null;
        }
    }

    public GameObject GetGiftOnDisable() {
        foreach(GameObject gift in list_Gift) {
            if(!gift.activeInHierarchy) {
                return gift;
            }
        }
        return AddGift();
    }

    public GameObject AddGift() {
        GameObject giftObject = Instantiate(obj_giftPrefab, this.gameObject.transform);
        giftObject.SetActive(false);
        list_Gift.Add(giftObject);
        return giftObject;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos;
        if (DataManager.Ins.gameSave.levelNormal > 1)
        {
            float x = Random.Range(GameConstants.flt_spawn2MinX, GameConstants.flt_spawn2MaxX);
            float z = Random.Range(GameConstants.flt_spawn2MinZ, GameConstants.flt_spawn2MaxZ);
            randomPos = new Vector3(x, 0.435f, z);
        }
        else
        {
            float x = Random.Range(GameConstants.flt_spawnMinX, GameConstants.flt_spawnMaxX);
            float z = Random.Range(GameConstants.flt_spawnMinZ, GameConstants.flt_spawnMaxZ);
            randomPos = new Vector3(x, 0.435f, z);
        }

        // tìm vị trí hợp lệ trên NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            // nếu không tìm được, thử lại đệ quy hoặc trả về randomPos (có thể fail)
            Debug.LogWarning("Không tìm thấy vị trí hợp lệ trên NavMesh, thử lại...");
            return GetRandomSpawnPosition();
        }
    }
}
