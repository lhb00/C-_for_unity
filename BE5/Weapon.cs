using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }; // 무기 타입, 데미지, 공속, 범위, 효과 변수 생성
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo; // 무기 스크립트에 전체 탄약, 현재 탄약 변수 생성
    public int curAmmo;
    
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos; // 총알, 탄피 관련된 변수 생성
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    void Start()
    {
        if(meleeArea != null)
        {
            meleeArea.enabled = false;
        }
    }

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // StopCoroutine() : 코루틴 정지 함수
            StartCoroutine("Swing"); // StartCoroutine() : 코루틴 실행 함수
            
        }
        // 현재 탄약을 조건에 추가하고, 발사했을 때 감소하도록 작성
        else if(type == Type.Range && curAmmo > 0) // 무기에서 타입으로 조건을 주어 다른 코루틴 실행하기
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() // IEnumerator : 열거형 함수 클래스
    {
        yield return new WaitForSeconds(0.45f); // yield : 결과를 전달하는 키워드 // 0.1초 대기
        // WaitForSeconds() : 주어진 수치만큼 기다리는 함수
        meleeArea.enabled = true; // trail Renderer와 BoxCollider를 시간차로 활성화 컨트롤
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.1f); ; // yield 키워드를 여러개 사용하여 시간차 로직 작성 가능
        // yield return null : 1프레임 대기
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f); ;
        trailEffect.enabled = false;

        // yield break로 코루틴 탈출 가능
    }

    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 (교차 실행)
    // Use() 메인루틴  + Swing() 코루틴 (Co-Op)

    IEnumerator Shot()
    {
        // 1. 총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation); // Instantiate() 함수로 총알 인스턴스화 하기
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50; // 인스턴스화된 총알에 속도 적용하기
        yield return null;

        // 2. 탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation); // Instantiate() 함수로 총알 인스턴스화 하기
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3); // 인스턴스화된 탄피에 랜덤한 힘 가하기
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
