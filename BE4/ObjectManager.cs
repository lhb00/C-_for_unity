using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLprefab; // 프리펩 변수 생성 후, 잊지말고 할당
    public GameObject enemyMprefab;
    public GameObject enemySprefab;
    public GameObject itemCoinprefab;
    public GameObject itemPowerprefab;
    public GameObject itemBoomprefab;
    public GameObject bulletPlayerAprefab;
    public GameObject bulletPlayerBprefab;
    public GameObject bulletEnemyAprefab;
    public GameObject bulletEnemyBprefab;

    GameObject[] enemyL; // 프리펩을 생성하여 저장할 배열 변수 생성
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;

    GameObject[] targetPool;

    void Awake()
    {
        enemyL = new GameObject[10]; // 한번에 등장할 개수를 고려하여 배열 길이 할당
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];

        Generate(); // 첫 로딩 시간 = 장면 배치 + 오브젝트 풀 생성
    }

    void Generate()
    {
        // 1. Enemy
        for(int index=0; index < enemyL.Length; index++)
        {
            enemyL[index] =  Instantiate(enemyLprefab); // Instantiate()로 생성한 인스턴스를 배열에 저장
            enemyL[index].SetActive(false); // Instantiate()로 생성 후엔 바로 비활성화
        } 

        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMprefab);
            enemyM[index].SetActive(false);
        }

        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySprefab);
            enemyS[index].SetActive(false);
        }

        // 2. Item
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinprefab);
            itemCoin[index].SetActive(false);
        }

        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerprefab);
            itemPower[index].SetActive(false);
        }

        for (int index = 0; index < itemBoom.Length; index++)
        {
            itemBoom[index] = Instantiate(itemBoomprefab);
            itemBoom[index].SetActive(false);
        }


        // 3. Bullet
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAprefab);
            bulletPlayerA[index].SetActive(false);
        }

        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBprefab);
            bulletPlayerB[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAprefab);
            bulletEnemyA[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBprefab);
            bulletEnemyB[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type) // 오브젝트 풀에 접근할 수 있는 함수 생성
    {
        switch(type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
        }

            for (int index = 0; index < targetPool.Length; index++)
            {
                if (!targetPool[index].activeSelf) // 활성화된 오브젝트에 접근하여 활성화 , 반환
                {
                    targetPool[index].SetActive(true);
                    return targetPool[index];
                }
            }

            return null;
    }

    public GameObject[] GetPool(string type) // 지정한 오브젝트 풀을 가져오는 함수 추가
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
        }

        return targetPool;
    }
}
