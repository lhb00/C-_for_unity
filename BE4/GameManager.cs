using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints; // 적 프리펨 배열과 생성 위치 배열 변수를 선언

    public float maxSpawnDelay; // 적 생성 딜레이 변수 선p
    public float curSpawnDelay;

    public GameObject player;

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f); // RandomRange()은 현재 사용하지 않는 함수
            // Range() : 정해진 범위 내의 랜덤 숫자 변환 (float, int)
            curSpawnDelay = 0; // 적 생성 후엔 꼭 딜레이 변수 0으로 초기화
        }
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3); // Range()함수는 매개변수에 의해 반환 타입 결
        int ranPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObjs[ranEnemy],
                    spawnPoints[ranPoint].position,
                    spawnPoints[ranPoint].rotation); // 랜덤으로 정해진 적 프리, 생성 위치로 생성 로직 작성
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; // 적 생성 직후에 플레이어 변수를 넘겨주는 것으로 프리펩은 이미 Scene에 올라온 오브젝트에 접근 불가능한 문제 해결
        if(ranPoint == 5 || ranPoint == 6) // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);// 속도 방향에 따라 적 비행기 회전 적용
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1); // 생성 위치에 따라 속도를 다르게 설정
        }

        else if(ranPoint == 7 || ranPoint == 8) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else // Front Spawn
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
        // 적 비행기 속도를 GameManager가 관리하도록 수정
    }

    public void RespawnPlayer() // 플레이어를 복귀시키는 로직은 매니저가 관리
    {
        Invoke("RespawnPlayerExe", 2f); // 플레이어 복귀는 시간 차를 두기 위해 Invoke() 사용
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
    }
}
