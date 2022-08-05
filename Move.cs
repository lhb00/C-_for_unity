using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Vector3 target = new Vector3(8, 1.5f, 0);

    void Update() // 본 파일의 함수는 모두 vector3 클래스에서 기본 제공되는 함수임.
    {   //1.MoveTowards
        /*transform.position = Vector3.MoveTowards(transform.position
            , target, 2f); // 등속이동, 매개변수는 (현재위치, 목표위치, 속도)임.*/

        //2.SmoothDamp
        /*Vector3 velo = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position
            , target, ref velo, 1f); // 부드러운 감속 이동, 매개변수는 (현재위치, 목표위치, 참조속도, 속도)임.
        // ref : 참조 접근, 실시간으로 바뀌는 값 적용 O */

        //3.Lerp
        /*transform.position = Vector3.Lerp(transform.position
            , target, 0.05f); // 선형 보간, SmoothDamp 보다 긴 감속시간. 마지막 매개변수에 비례하여 속도 증가. 최대값 1.*/

        //4.SLerp
        transform.position = Vector3.Slerp(transform.position
            , target, 0.1f); //구면 선형 보간, 호를 그리며 이동
    }
}