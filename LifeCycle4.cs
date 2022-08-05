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
    
    // Time.deltaTime ����
    // Translate: ���Ϳ� ���ϱ� ex.transform.Translate(Vec*Time.deltaTime);
    // Vector : �ð� �Ű������� ���ϱ� ex. Vector3.Ler(Vec1, Vec2, T* Time.deltaTime);
    // Time.deltaTime: ���� �������� �Ϸ���� �ɸ� �ð�
    // deltaTime ���� �������� ������ ũ��, �������� ������ �۴�.
}
