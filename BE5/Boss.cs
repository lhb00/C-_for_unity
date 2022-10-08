using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    // 미사일 관련 변수 생성
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public bool isLook; // 플레이어 바라보는 플래그 bool 변수 추가

    Vector3 lookVec; // 플레이어 움직임 예측 벡터 변수 생성
    Vector3 tauntVec;

    void Awake()
    {
        // 초기화 로직을 자식 스크립트의 Awake() 함수에 작성
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Update()
    {
        if(isDead)
        {
            StopAllCoroutines(); // 죽음 플래그 bool 변수를 활용하여 패턴 정지 로직 작성
            return;
        }

        if (isLook)
        {
            // 플레이어 입력값으로 예측 벡터값 생성
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec); // 점프공격 할 때 목표지점으로 이동하도록 로직 추가
    }

    // 행동 패턴을 결정해주는 코루틴 생성
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5); // 행동 패턴을 만들기 위해 Random.Range() 함수 호출
        switch(ranAction) // switch문에서 break를 생략하여 조건을 늘릴 수 있다.
        {
            case 0:
            case 1:
                // 미사일 발사 패턴
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                // 돌 굴러가는 패턴
                StartCoroutine(RockShot());
                break;
            case 4:
                // 점프 공격 패턴
                StartCoroutine(Taunt());
                break;
        }
    }

    // 3종 패턴들을 담당할 코루틴을 생성 + 각 패턴에 맞는 애니메이션을 SetTrigger() 함수로 실행 \
    // 패턴이 끝나면 다음 패턴을 위해 다시 Think() 코루틴 실행

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissleA = Instantiate(missile, missilePortA.position, missilePortA.rotation); // Instantiate() 함수로 미사일 생성
        BossMissile bossMissileA = instantMissleA.GetComponent<BossMissile>(); // 미사일 스크립트까지 접근하여 목표물 설정해주기
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissleB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissleB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false; // 기 모을 때는 바라보기 중지하도록 플래그 설정
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true; // 꺼둔 바라보기 플래그 bool 변수를 되돌려놓는 것 잊지말기
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec; // 점프공격을 할 위치를 변수에 저장

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false; // 콜라이더가 플레이어를 밀지 않도록 비활성화
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true; // 일정 시간 지난 후, 공격 범위 콜라이더 활성화

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
