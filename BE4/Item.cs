using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type; // 아이템 타입을 위한 변수 추가
    Rigidbody2D rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * 3f; // 아이템 속도 추가
    }

}
