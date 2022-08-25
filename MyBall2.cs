using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBall : MonoBehaviour
{
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>(); // GetComponent<T> => 자신의 T타입 컴포넌트를 가져옴.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /* rigid.velocity = Vector3.forward; */  // 1. 속력 바꾸기 // velocity:현재 이동 속도 
        // Rigidbody에 관한건 물리 현상이므로 FixedUpdate에 작성해주어야함!

        /* if (Input.GetButtonDown("Jump")) {
            rigid.AddForce(Vector3.up * 50, ForceMode.Impulse);  // ForceMode: 힘을 주는 방식(가속, 무게 반영됨) // 무게 값보다 더 큰 값을 힘으로 줘야 움직일 수 있음.
            // AddForce 방향으로 Velocity가 계속 증가
            Debug.Log(rigid.velocity); 
        } */
        Vector3 vec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rigid.AddForce(vec, ForceMode.Impulse); // 2. 힘을 가하기

        // 3. 회전력
        /* rigid.AddTorque(Vector3.down); */ // AddTorque(Vec) : Vec 방향을 축으로 회전력이 생김.
        // Vec를 축으로 삼기 때문에 이동 방향에 주의할 것!
    }
    /* private void OnTriggerEnter(Collider other)
    {

    } // OnTriggerEnter: 콜라이더가 충돌이 시작할 때 호출 */


    private void OnTriggerStay(Collider other) // TriggerStay: 콜라이더가 계속 충돌하고 있을 때 호출
    {
        if(other.name=="Cube")
            rigid.AddForce(Vector3.up * 2, ForceMode.Impulse);
    }
    
    /* private void OnTriggerExit(Collider other)
    {

    } // OnTriggerExit: 콜라이더가 충돌이 끝날 때 호출 */
}
