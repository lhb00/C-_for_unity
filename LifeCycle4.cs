using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        Vector3 vec = new Vector3(
            Input.GetAxisRaw("Horizontal") * Time.deltaTime ,
            Input.GetAxisRaw("Vertical") * Time.deltaTime );
        transform.Translate(vec);
    }
    
    // Time.deltaTime 사용법
    // Translate: 벡터에 곱하기 ex.transform.Translate(Vec*Time.deltaTime);
    // Vector : 시간 매개변수에 곱하기 ex. Vector3.Ler(Vec1, Vec2, T* Time.deltaTime);
    // Time.deltaTime: 이전 프레임의 완료까지 걸린 시간
    // deltaTime 값은 프레임이 적으면 크고, 프레임이 많으면 작다.
}
