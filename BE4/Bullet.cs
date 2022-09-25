using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg; // public으로 설정한 변수들의 초기 값을 잊지말고 꼭 설정

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet") // 총알 제거 경계를 위한 새로운 태그로 조건 걸기
            Destroy(gameObject); // Destroy(): 매개변수 오브젝트를 삭제하는 함수
    }
}
