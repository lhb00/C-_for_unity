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
    public GameManager gameManager;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex; // 패턴 흐름에 필요한 변수 생성
    public int curPatternCount;
    public int[] maxPatternCount;

    // 변수가 준비되었으면 함수로 로직 작성

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B")
            anim = GetComponent<Animator>();
    }

    void OnEnable() // OnEnable() : 컴포넌트가 활성화 될 때 호출되는 생명주기함수
    {
        switch(enemyName)
        {
            case "B":
                health = 1000;
                Invoke("Stop", 2);
                break;
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

    void Stop()
    {
        if (!gameObject.activeSelf) // Stop 함수가 두 번 사용되지 않도록 조건 추가
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1; // 현재 패턴이 패턴 갯수를 넘기면 0으로 돌아오는 로직 작성
        curPatternCount = 0; // 패턴 변경 시, 패턴 실행 횟수 변수 초기화

        switch (patternIndex)
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireForward() // 각 패턴별로 함수를 생성 // 첫번째 패턴 : 일직선 4발 발사
    {
        if (health <= 0)
            return;
        // Fire Forward 4 bullet
        GameObject bulletR = objectManager.MakeObj("bulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("bulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse); // normalized : 벡터가 단위 값(1)로 변환된 변.
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        GameObject bulletL = objectManager.MakeObj("bulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("bulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        // Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) // 각 패턴별 횟수를 실행하고 다음 패턴으로 넘어가도록 구현
            Invoke("FireForward", 2f);
        else
            Invoke("Think", 3f);
    }

    void FireShot() // 두번째 패턴 : 샷건 형태의 방사형 공격
    {
        if (health <= 0)
            return;

        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position; // 플레이어에게 쏘기 위해 플레이어 변수 필요
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f)); // 위치가 겹치지 않게 랜덤 벡터를 더하여 구현
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) // 각 패턴별 횟수를 실행하고 다음 패턴으로 넘어가도록 구현
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }

    void FireArc() // 부채 형태의 연속 공격
    {
        if (health <= 0)
            return;

        // Fire Arc Continube Fire
        GameObject bullet = objectManager.MakeObj("bulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf. PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1); // Mathf.Sin() : 삼각함수 Sin
        // Mathf.PI : 원주율 상수 (3.14...)
        // 원의 둘레값을 많이 줄수록 빠르게 파형을 그림
        // Sin <-> Cos 발사 각도가 다를 뿐, 모양은 동일함
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) // 각 패턴별 횟수를 실행하고 다음 패턴으로 넘어가도록 구현
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    void FireAround() // 네번째 패턴 : 원 형태의 전체 공격
    {
        if (health <= 0)
            return;

        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount%2==0 ? roundNumA : roundNumB; // 패턴 횟수에 따라 생성되는 총알 갯수 조절로 난이도 상승
        for (int index = 0; index < roundNumA; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum)); // 생성되는 총알의 순번을 활용하여 방향 결정
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum +  Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
       

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) // 각 패턴별 횟수를 실행하고 다음 패턴으로 넘어가도록 구현
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    void Update()
    {
        if (enemyName == "B")
            return;
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
        if(enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f); // 바꾼 스프라이트를 돌리기 위해 시간차 함수 호출
        }

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore; // Enemy가 파괴될 때, Player의 Score에 더해주는 로직 추가

            // Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
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
            transform.rotation = Quaternion.identity; // Quaterion.identity  : 기본 회전값 = 0
            gameManager.CallExplosion(transform.position, enemyName);

            // Boss Kill
            if (enemyName == "B")
                gameManager.StageEnd();
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision) // OnTriggerEnter2D를 통하여 이벤트 로직 작성
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
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
