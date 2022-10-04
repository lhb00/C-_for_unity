using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }; // 무기 타입, 데미지, 공속, 범위, 효과 변수 생성
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // StopCoroutine() : 코루틴 정지 함수
            StartCoroutine("Swing"); // StartCoroutine() : 코루틴 실행 함수
            
        }
    }

    IEnumerator Swing() // IEnumerator : 열거형 함수 클래스
    {
        yield return new WaitForSeconds(0.1f); // yield : 결과를 전달하는 키워드 // 0.1초 대기
        // WaitForSeconds() : 주어진 수치만큼 기다리는 함수
        meleeArea.enabled = true; // trail Renderer와 BoxCollider를 시간차로 활성화 컨트롤
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f); ; // yield 키워드를 여러개 사용하여 시간차 로직 작성 가능
        // yield return null : 1프레임 대기
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f); ;
        trailEffect.enabled = false;

        // yield break로 코루틴 탈출 가능
    }

    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 (교차 실행)
    // Use() 메인루틴  + Swing() 코루틴 (Co-Op)
}
