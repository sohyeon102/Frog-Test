using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearObstacle : MonoBehaviour
{
    public GameObject bear; 
    public Transform[] spawnPaths; 
    public Transform[] destinationPoints; 
    public float minSpawnDelay = 2f; 
    public float maxSpawnDelay = 5f; 

    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float destroyAfter = 10f;
    
    void Start()
    {
        StartCoroutine(SpawnBearRoutine());
    }
    
    IEnumerator SpawnBearRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            //SpawnBear();
        }
    }

    /*// 곰 생성
    void SpawnBear()
    {
        int randomIndex = Random.Range(0, spawnPaths.Length);
        Transform spawnPoint = spawnPaths[randomIndex];
        Transform destination = destinationPoints[randomIndex];
        
        GameObject spawnedBear = Instantiate(bear, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"곰 생성위치: {randomIndex + 1}");
        
        BearMove mover = spawnedBear.GetComponent<BearMove>();
        if (mover != null)
        {
            mover.Init(destination.position, moveSpeed, destroyAfter);
        }
    }*/
}
    
    // 충돌 처리 BearHitbox. 플레이어와 충돌 시 플레이어 넉백 처리
    // 일정 시간 후 Destroy

