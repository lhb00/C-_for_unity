using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCan : MonoBehaviour
{
    public float rotateSpeed;
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World); // Rotate(Vector3) : 매개변수 기준으로 회전시키는 함수
        // 오버로드: 이름은 같지만 매개변수가 다른 함수를 호출
    }
}
