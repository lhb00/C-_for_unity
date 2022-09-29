using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    // 적 비행기의 구성 요소를 변수로 구체화
    public int enemyScore; // Enemy가 각자 가질 수 있는 Score 변수 생성
    public float speed;
    public int health;
    public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA; // 플레이어의 쏘는 로직을 재활용
    public GameObject bulletObjB;
    public GameObject itemCoin; // 프리펩을 저장할 변수 생성
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;

    // 변수가 준비되었으면 함수로 로직 작성

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() // OnEnable() : 컴포넌트가 활성화 될 때 호출되는 생명주기함수
    {
        switch(enemyName)
        {
            case "L":
                health = 40;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 3;
                break;
        }
    }

    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if(enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position; // 플레이어에게 쏘기 위해 플레이어 변수 필요
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            Rigidbody2D rigidR= bulletR.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f); // 플레이어에게 쏘기 위해 플레이어 변수 필요
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse); // normalized : 벡터가 단위 값(1)로 변환된 변.

            GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime; // 딜레이 변수에 Time.deltaTime을 계속 더하여 시간 계산
    }

    public void OnHit(int dmg)
    {
        if (health <= 0) // 아이템이 다중으로 만들어지지 않도록 예외처리
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f); // 바꾼 스프라이트를 돌리기 위해 시간차 함수 호출
        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore; // Enemy가 파괴될 때, Player의 Score에 더해주는 로직 추가

            // Random Ratio Item Drop
            int ran = Random.Range(0, 10);
            if (ran < 3) // 랜덤 숫자를 이용하여 아이템 확률 로직 작성 // Not Item 30%
            {
                Debug.Log("Not Item");
            }

            else if (ran < 6) // Coin // Coin 30%
            {
                GameObject itemCoin = objectManager.MakeObj("itemCoin");
                itemCoin.transform.position = transform.position;
            }

            else if (ran < 8) // Power // Power 20%
            {
                GameObject itemPower = objectManager.MakeObj("itemPower");
                itemPower.transform.position = transform.position;
            }

            else if (ran < 10) // Boom // Boom 20%
            {
                GameObject itemBoom = objectManager.MakeObj("itemBoom");
                itemBoom.transform.position = transform.position;
            }
            gameObject.SetActive(false); // Destory()는 SetActive(false)로 교체
            transform.rotation = Quaternion.identity; // Quaterion.identuty  : 기본 회전값 = 0
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision) // OnTriggerEnter2D를 통하여 이벤트 로직 작성
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false); // 총알과 마찬가지로 바깥으로 나간 후에는 삭제
            transform.rotation = Quaternion.identity;
        }

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);


            collision.gameObject.SetActive(false); // 피격시 플레이어 총알도 삭제하는 로직 추가
        }
    }
}
