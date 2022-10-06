using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    void Start()
    {
        StartCoroutine(Explosion());
    }

    // 시간차 폭발을 위해 코루틴 선언
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero; // 물리적 속도를 모두 Vector3.zero로 초기화
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false); // 수류탄 매쉬는 비활성화, 폭발은 활성화 하기
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy")); // SphereCastAll : 구체 모양의 레이캐스팅(모든 오브젝트)
        foreach(RaycastHit hitObj in rayHits) // foreach 문으로 수류탄 범위 적들의 피격함수를 호출
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5); // 수류탄은 파티클이 사라지는 시간을 고려하여 Destroy() 호출
    }
}
