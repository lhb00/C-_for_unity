using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nav 관련 클래스는 UnityEngine.AI 네임스페이스 사용

public class Enemy : MonoBehaviour
{
    public int maxHealth; // 체력과 컴포넌트를 담을 변수 선언
    public int curHealth;
    public Transform target;
    public bool isChase; // 추적을 결정하는 bool 변수 추가

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material; // Material은 Mesh Renderer 컴포넌트에서 접근 가능
        // MashRenderer를 가져오는 GetComponent()
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if(isChase)
            nav.SetDestination(target.position); // SetDestination() : 도착할 목표 위치 지정 함수
    }

    void FreezeVelocity()
    {
        if(isChase)
        {
            rigid.velocity = Vector3.zero; // 물리력이 NavAgent 이동을 방해하지 않도록 로직 작성
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other) // OnTriggerEnter() 함수에 태그 비교 조건을 작성
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>(); // 충돌 상대의 스크립트를 가져와 damage값을 체력에 적용
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position; // 현재 위치에 피격 위치를 빼서 반작용 방향 구하기

            StartCoroutine(OnDamage(reactVec, false)); // 로직을 담을 코루틴 생성
        }

        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject); // 총알의 경우, 적과 닿았을 때 삭제되도록 Destroy() 호출
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos) // 피격함수 로직은 이전과 동일
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade) // 수류탄만의 리액션을 위해 bool 매개변수 추가
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0) // 남아있는 체력을 조건으로 피격 결과 로직 작성
        {
            mat.color = Color.white;
        }

        else
        {
            mat.color = Color.gray;
            gameObject.layer = 14; // 레이어 번호를 그대로 gameObject.layer에 적용
            // 적이 죽는 시점에서도 애니메이션과 플래그 셋팅
            isChase = false;
            nav.enabled = false; // 사망 리액션을 유지하기 위해 NavAgent를 비활성
            anim.SetTrigger("doDie");

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;
                rigid.freezeRotation = false; // 수류탄에 의한 사망 리액션은 큰 힘과 회전을 추가
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }

            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse); // AddForce() 함수로 넉백 구현하기
            }

            Destroy(gameObject, 4);
        }
    }
}
