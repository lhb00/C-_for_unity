using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    // 적 비행기의 구성 요소를 변수로 구체화
    public float speed;
    public int health;
    public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA; // 플레이어의 쏘는 로직을 재활용
    public GameObject bulletObjB;
    public GameObject player;

    SpriteRenderer spriteRenderer;

    // 변수가 준비되었으면 함수로 로직 작성

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position; // 플레이어에게 쏘기 위해 플레이어 변수 필요
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.3f, transform.rotation);
            Rigidbody2D rigidR= bulletR.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f); // 플레이어에게 쏘기 위해 플레이어 변수 필요
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse); // normalized : 벡터가 단위 값(1)로 변환된 변.

            GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.3f, transform.rotation);
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

    void OnHit(int dmg)
    {
        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f); // 바꾼 스프라이트를 돌리기 위해 시간차 함수 호출
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision) // OnTriggerEnter2D를 통하여 이벤트 로직 작성
    {
        if (collision.gameObject.tag == "BorderBullet")
            Destroy(gameObject); // 총알과 마찬가지로 바깥으로 나간 후에는 삭제

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);


            Destroy(collision.gameObject); // 피격시 플레이어 총알도 삭제하는 로직 추가
        }
    }
}
