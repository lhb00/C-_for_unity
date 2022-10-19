// 1. if-then으로 구현된 FSM
// if-then 대신 switch-case 조건문으로 작성할 수 있지만, 결과는 같다. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어가 할 수 있는 행동
    public enum PlayerState { Idle = 0, Walk, Run, End }

    private PlayerState playerState;

    private void Awake()
    {
        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        // 1 ~ 4 숫자키를 눌러 상태 변경
        if (Input.GetKeyDown("1")) ChangeState(PlayerState.Idle);
        else if (Input.GetKeyDown("2")) ChangeState(PlayerState.Walk);
        else if (Input.GetKeyDown("3")) ChangeState(PlayerState.Run);
        else if (Input.GetKeyDown("4")) ChangeState(PlayerState.End);
    }

    // 플레이어의 행동을 newState로 변경
    private void ChangeState(PlayerState newState)
    {
        // 열거형 변수.ToString()을 하게 되면 열거형에 정의된 변수 이름 string을 반환
        // playerState = PlayerState.Idle; 일 때 playerState.ToString()을 하면 "Idle" 반환

        // 열거형에 정의된 상태와 동일한 이름의 코루틴 메소드를 정의
        // playerState의 현재 상태에 따라 코루틴 메소드 실행

        // 이전 상태의 코루틴 종료
        StopCoroutine(playerState.ToString());

        // 새로운 상태로 변경
        playerState = newState;

        // 현재 상태의 코루틴 실행
        StartCoroutine(playerState.ToString());
    }

    private IEnumerator Idle()
    {
        // 상태로 진입할때 1회만 호출하는 내용
        Debug.Log("충분한 스트레칭을 해주세요!");
        Debug.Log("최소 7시간의 수면을 취해주세요!");

        while(true)
        {
            // 상태일 때 계속 호출되는 내용
            Debug.Log("운동 전 충분한 수분을 섭취해주세요!");
            yield return null;
        }
    }

    private IEnumerator Walk()
    {
        Debug.Log("잠시 후 달리기를 시작합니다! 슬슬 움직여 볼까요?");

        while(true)
        {
            Debug.Log("운동을 일시정지 합니다!");
            yield return null;
        }
    }

    private IEnumerator Run()
    {
        Debug.Log("지속 가능한 페이스를 유지하세요!");

        while (true)
        {
            Debug.Log("현재까지 nkm를 달렸습니다! 도달 시간은 m분 s초 입니다. 현재 페이스는 m분 s초 입니다.");
            yield return null;
        }
    }

    private IEnumerator End()
    {
        Debug.Log("운동을 종료합니다.");

        while (true)
        {
            Debug.Log("완주하지 못했다고 너무 실망하지 마세요!");
            yield return null;
        }
    }
}
