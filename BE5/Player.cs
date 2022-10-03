using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 인스펙터 창에서 설정할 수 있도록 public 변수 추가
    float hAxis; // Input Axis 값을 받을 전역변수 선언
    float vAxis;
    bool wDown;

    Vector3 moveVec;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); // Animator 변수를 GetComponentINChildren()으로 초기화
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); // GetAxisRaw() : Axis 값을 정수로 반환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk"); // Shift는 누를 때만 작동되도록 GetButton() 함수 사용

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // normalized : 방향 값이 1로 보정된 벡터


        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // transform 이동은 반드시 Time.deltaTime 넣기
        // bool 형태 조건 ? true일 때 값 : false일 때 값(삼항연산자)

        anim.SetBool("isRun", moveVec != Vector3.zero); // SetBool() 함수로 파라미터 값을 설정하기
        anim.SetBool("isWalk", wDown);

        transform.LookAt(transform.position + moveVec); // LookAt() : 지정된 벡터를 향해서 회전시켜주는 함수
    }
}
