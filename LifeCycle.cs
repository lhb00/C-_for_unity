using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Awake() // 게임 오브젝트 생성 할 때, 한번만 실행
    {
        Debug.Log("플레이어 데이터가 준비되었습니다.");
    }

    void OnEnable() //업데이트 활성화
    {
        Debug.Log("플레이어가 로그인했습니다.");
    }

    void Start() // 업데이트 시작 직전, 최초 실행
    {
        Debug.Log("사냥 장비를 챙겼습니다.");
    }

    void FixedUpdate() //물리 연산 업데이트, 고정된 주기로 CPU를 사용
    {
        Debug.Log("이동~");
    }
    
    void Update() //게임 로직 업데이트, 보통 FixedUpdate()는 60프레임 주기이기 때문에 이 Update()가 더 자주 실행됨.
    {
        Debug.Log("몬스터 사냥");
    }

    void LateUpdate() // 모든 업데이트가 끝난 이후
    {
        Debug.Log("경험치 획득.");
    }

    void OnDisable() //업데이트 비활성화
    {
        Debug.Log("플레이어가 로그아웃했습니다.");
    }

    void OnDestroy() // 로직 해체
    {
        Debug.Log("플레이어 데이터를 해제하였습니다.");
    }
}
