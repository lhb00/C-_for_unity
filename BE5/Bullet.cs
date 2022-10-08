using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock; // bool 변수를 추가하여 조건 추가

    void OnCollisionEnter(Collision collision) // OnCollisionEnter()에서 각각 충돌 로직 작성
    {
        if(!isRock && collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }


    }

    // 총알을 위해 OnTriggerEnter() 함수 로직 생성
    void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall") // 근접공격 범위가 파괴되지 않도록 조건 추가
        {
            Destroy(gameObject);
        }
    }
}
