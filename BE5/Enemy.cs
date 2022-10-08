using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nav 관련 클래스는 UnityEngine.AI 네임스페이스 사용

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C , D}; // enum으로 타입을 나누고 그것을 지정할 변수 생성
    // enum에서 보스 타입 추가
    public Type enemyType;
    public int maxHealth; // 체력과 컴포넌트를 담을 변수 선언
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet; // 미사일 프리펩을 담아둘 변수 생성
    public bool isChase; // 추적을 결정하는 bool 변수 추가
    public bool isAttack;
    public bool isDead; // 죽었을 때를 알기 위한 플래그 bool 변수 추가

    // 부모 클래스의 요소를 사용하려면 public 변수여야함.
    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs; // 피격 이펙트를 플레이어처럼 모든 메테리얼로 변경
    public NavMeshAgent nav;
    public Animator anim;

    // Awake() 함수는 자식 스크립트만 단독 실행!
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>(); // Material은 Mesh Renderer 컴포넌트에서 접근 가능
        // MashRenderer를 가져오는 GetComponent()
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if(enemyType!=Type.D)
            Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        // 기존 로직은 목표만 잃어버리는 것이므로 이동이 유지됨
        if(nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position); // SetDestination() : 도착할 목표 위치 지정 함수
            nav.isStopped = !isChase; // isStopped를 사용하여 완벽하게 멈추도록 작성
        }
            
    }

    void FreezeVelocity()
    {
        if(isChase)
        {
            rigid.velocity = Vector3.zero; // 물리력이 NavAgent 이동을 방해하지 않도록 로직 작성
            rigid.angularVelocity = Vector3.zero;
        }
    }

    // 타겟팅을 위한 함수 생성

    void Targeting()
    {
        if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0; // SphereCast()의 반지름, 길이를 조정할 변수 선언
            float targetRange = 0;
            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f; // SphereCast()의 반지름, 길이를 조정할 변수 선언
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1.5f; // SphereCast()의 반지름, 길이를 조정할 변수 선언
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f; // SphereCast()의 반지름, 길이를 조정할 변수 선언
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack) // rayHit 변수에 데이터가 들어오면 공격 코루틴 실행
            {
                StartCoroutine(Attack());
            }
        }

        
    }

    IEnumerator Attack()
    {
        isChase = false; // 먼저 정지를 한 다음, 애니메이션과 함께 공격범위 활성화
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch(enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse); // AddForce()로 돌격 구현
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero; // velocity를 Vector3.zero로 속도 제어
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation); // Instantiate() 함수로 미사일 인스턴스화
                Rigidbody rigidbullet = instantBullet.GetComponent<Rigidbody>();
                rigidbullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }
        
        isChase = true; // 먼저 정지를 한 다음, 애니메이션과 함께 공격범위 활성화
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targeting();
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
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0) // 남아있는 체력을 조건으로 피격 결과 로직 작성
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }

        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;
            gameObject.layer = 14; // 레이어 번호를 그대로 gameObject.layer에 적용
            // 적이 죽는 시점에서도 애니메이션과 플래그 셋팅
            isDead = true;
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

            if(enemyType != Type.D)
                Destroy(gameObject, 4);
        }
    }
}
