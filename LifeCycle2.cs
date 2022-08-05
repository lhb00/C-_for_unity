using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Update() //게임 로직 업데이트, 보통 FixedUpdate()는 60프레임 주기이기 때문에 이 Update()가 더 자주 실행됨.
    {
        if (Input.anyKeyDown)//게임 내 입력을 관리. anyKeyDown은 아무 입력을 최초로 받을 때 true
            Debug.Log("플레이어가 아무 키를 눌렀습니다.");
        if (Input.GetKeyDown(KeyCode.Return))//게임 내 입력을 관리. GetKeyDown은 키보드 입력을 받으면 true
            Debug.Log("아이템을 구입하였습니다.");

        if (Input.GetMouseButtonDown(0)) 
            Debug.Log("미사일 발사!");

        if (Input.GetMouseButton(0)) //마우스 버튼 입력 받으면 true
            Debug.Log("미사일 모으는 중...");

        if (Input.GetMouseButtonUp(0))
            Debug.Log("슈퍼 미사일 발사!!");

        if (Input.GetKey(KeyCode.LeftArrow))
            Debug.Log("왼쪽으로 이동 중");

        if (Input.GetKeyUp(KeyCode.RightArrow))
            Debug.Log("오른쪽 이동을 멈추었습니다.");

        if (Input.GetButtonDown("SuperFire"))
            Debug.Log("필살기!");

        if (Input.GetButton("Fire1")) // Input 버튼 입력을 받으면 true
            Debug.Log("점프 모으는 중...");

        if (Input.GetButtonUp("Fire1"))
            Debug.Log("슈퍼 점프!!");
        
        if (Input.GetButton("Horizontal"))
            Debug.Log("횡 이동 중..." + Input.GetAxisRaw("Horizontal")); //수평, 수직 버튼 입력을 받으면 float, raw를 추가하면 무조건 1 단위로

        if (Input.GetButton("Vertical"))
        {
            Debug.Log("종 이동 중..." + Input.GetAxisRaw("Vertical"));
        }
    }
}
