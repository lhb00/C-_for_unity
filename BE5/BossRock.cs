using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    // 리지드바디 변수와 회전파워, 크기 숫자값 변수 생성
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot; // 기를 모으고 쏘는 타이밍을 관리할 bool 변수 추가

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    // 쏘는 타이밍을 관리할 코루틴 생성
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        while(!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue; // While문에서 증가된 값을 트랜스폼, 리지드바디에 적용
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null; // While문에는 꼭 yield return null 포함(안하면 게임이 정지됨)
        }
    }
}
