using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 4방향 경계에 닿았다는 플래그 변수 추가
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    // 플레이어 로직에 목숨과 점수 변수를 추가
    public int life;
    public int score;
    public float speed;
    public int maxBoom; 
    public int boom; // 폭탄도 파워처럼 최대값과 현재값 변수 추가
    public int power;
    public int maxPower; // 파워는 최대값을 설정하여 구현
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA; // 총알 프리펩을 저장할 변수 생성
    public GameObject bulletObjB;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;
    public bool isHit; // 피격 중복을 방지하기 위한 bool 변수 추가
    public bool isBoomTime;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    void Move() // Update 함수의 로직을 함수로 분리
    {
        // Input.GetAxisRaw()를 통한 방향 값 추출
        float h = Input.GetAxisRaw("Horizontal");
        // 플래그 변수를 사용하여 경계 이상 넘지 못하도록 값 제한
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; // transform 이동에는 Time.DeltaTime을 꼭 사용하ㄱ

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")) // ButtonDown, ButtonUp일때만 함수 호출
        {
            anim.SetInteger("Input", (int)h); // SetInteger()로 Input값 전달
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1")) // Input.GetButton()으로 발사 버튼 적용
            return;

        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:

                // Power One
                GameObject bullet = objectManager.MakeObj("bulletPlayerA"); // Instantiate(): 매개변수 오브젝트를 생성하는 함수
                // Instantiate()를 모두 오브젝트 풀링으로 교체
                bullet.transform.position = transform.position;
                // 위치, 회전 매개변수는 플레이어 transform을 사용
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>(); // 리지드바디를 가져와 Addforce()로 총알 발사 로직 작성
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                // Power One
                GameObject bulletR = objectManager.MakeObj("bulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f; // Vector3.right, left 단위벡터를 더해 위치 조절
                GameObject bulletL = objectManager.MakeObj("bulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>(); 
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 3:
                GameObject bulletRR = objectManager.MakeObj("bulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                GameObject bulletCC = objectManager.MakeObj("bulletPlayerB");
                bulletCC.transform.position = transform.position;
                GameObject bulletLL = objectManager.MakeObj("bulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;


                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }



        curShotDelay = 0; // 총알을 쏜 다음에는 딜레이 변수 0으로 초기화
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime; // 딜레이 변수에 Time.deltaTime을 계속 더하여 시간 계산
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2")) // Input을 통한 폭탄 함수로 분리
            return;
        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

        // 1. Effect visible
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f); // 폭탄 스프라이트는 Invoke()로 시간차 비활성화

        // 2, Remove Enemy
        GameObject[] enemiesL = objectManager.GetPool("EnemyL"); // FindGameObjectsWithTag : 태그로 장면의 모든 오브젝트를 추출
        // Find 계열 함수를 오브젝트 풀링으로 교체
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");
        for (int index = 0; index < enemiesL.Length; index++)
        {
            if(enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

        }

        // 3. Remove Enemy Bullet
        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
                bulletsA[index].SetActive(false);
        }

        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
                bulletsB[index].SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // OnTriggerEnter2D로 플래그 세우기
    {
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet") // OnTriggerEnter2D 안에 적에 대한 로직 추가
        {
            if (isHit)
                return; // bool 변수와 return 키워드로 중복 피격 방지

            isHit = true;
            life--; // OnTriggerEnter()에서 목숨 로직 추가
            gameManager.UpdateLifeIcon(life);

            if(life == 0)
            {
                gameManager.GameOver();// 목숨이 다하면 GameOver 로직 실행
            }

            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }

        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case "Coin":
                    score += 1000;
                    break;

                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                        power++;
                    break;

                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision) // OnTriggerExit2D로 플래그 지우기 
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }
}
