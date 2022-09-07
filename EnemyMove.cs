using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove; // 행동지표를 결정할 변수를 하나 생성
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();

        Invoke("Think" , 5); // Invoke() : 주어진 시간이 지난 뒤, 지정된 함수를 실행해주는 함수 // 사용 이유는 딜레이 없이 재귀 함수를 사용하는 것은 CPU에 아주 안좋기 때문
    }

    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        
        // Platform check // 이동하는 경로의 상태를 예측해야 하므로 앞을 체크해야함.
        Vector2 frontVec = new Vector2(rigid.position.x +  nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider == null) {
                Turn();
            } 
    }
    
    void Think() // 행동지표를 바꿔줄 함수를 하나 생성
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2); // Random: 랜덤 수를 생성하는 로직 관련 클래스 // Range(): 최소~최대 범위의 랜덤 수 생성(최대 제외) // Python이랑 똑닮았네 이건;;

        // Sprite Animation
        anim.SetInteger("walkSpeed", nextMove);
        
        // Flip Sprite
        if(nextMove !=0)
            spriteRenderer.flipX = nextMove == 1;

        // Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        
        Invoke("Think" , nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke(); // CancelInvoke(): 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        Invoke("Think" , 5);
    }
}
