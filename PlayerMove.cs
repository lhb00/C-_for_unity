using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        // Jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        // Stop Speed
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); // Normalized: 벡터 크기를 1로 만든 상태(단위 벡터)
        }

        // Direction Sprite
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if(Mathf.Abs(rigid.velocity.x) < 0.3) // Mathf: 수학 관련 함수를 제공하는 클래스
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    void FixedUpdate()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if(rigid.velocity.x > maxSpeed) // Velocity: rigidbody의 현재 속도 // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        
        // Landing Platform
        if(rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); // DrawRay(): 에디터 상에서만 Ray를 그려주는 함수 // Raycast: 오브젝트 검색을 위해 Ray를 쏘는 방식
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform")); // RaycastHit: Ray에 닿은 오브젝트 // LayerMask: 물리 효과를 구분하는 정수값
            // GetMask() : 레이어 이름에 해당하는 정수값을 리턴하는 함수
            if(rayHit.collider != null) {
                if(rayHit.distance < 0.5f) // distance: Ray에 닿았을 때의 거리
                    anim.SetBool("isJumping", false);
            } // RaycastHit 변수의 collider로 검색 확인 가능
        }
    }
}
