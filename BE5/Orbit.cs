using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // 공전 목표, 공전 속도, 목표와의 거리 변수 생성
    public Transform target;
    public float orbitSpeed;
    Vector3 offSet;

    void Start()
    {
        offSet = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offSet;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime); // RotateAround() : 타겟 주위를 회전하는 함수
        // RotateAround()는 목표가 움직이면 일그러지는 단점이 있음.
        offSet = transform.position - target.position; // RotateAround() 후의 위치를 가지고 목표와의 거리를 유지
    }
}
