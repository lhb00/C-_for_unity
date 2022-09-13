using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager; // 플레이어에서 매니저 변수를 만들어 점수 변수에 접근
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action) 
    {
        switch (action) {
            case "JUMP":
                audioSource.clip = audioJump;
                audioSource.Play();
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                audioSource.Play();
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                audioSource.Play();
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                audioSource.Play();
                break;
            case "DIE":
                audioSource.clip = audioDie;
                audioSource.Play();
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                audioSource.Play();
                break;
            
        }   
    }
    void Update() 
    {
        // Jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            // Sound
            PlaySound("JUMP");
        }
        // Stop Speed
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); // Normalized: 벡터 크기를 1로 만든 상태(단위 벡터)
        }

        // Direction Sprite
        if(Input.GetButton("Horizontal")) // GetButtonDown은 키 입력이 겹치는 구간에서 문제 발생 // GetButton으로 변경
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy") {
            // Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y) // 몬스터 보다 위에 있음 + 아래로 낙하 중 = 밟음
            {
                OnAttack(collision.transform);
            }
            // Damaged
            else 
            {
                OnDamaged(collision.transform.position);
            }
        }

        else if(collision.gameObject.tag == "Spike") {
            OnDamaged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze"); // Contains(비교문): 대상 문자열에 비교문이 있으면 true
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if(isBronze)
                gameManager.stagePoint += 50;

            else if(isSilver)
                gameManager.stagePoint += 100;
        
            else if(isGold)
                gameManager.stagePoint += 300;

            // Deactive Item
            collision.gameObject.SetActive(false);

            // Sound
            PlaySound("ITEM");
        }

        else if(collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
            // Sound
            PlaySound("FINISH");
        }
    }

    void OnAttack(Transform enemy) // OnAttack() 함수에 몬스터의 죽음 관련 함수를 호출
    {
        // Point
        gameManager.stagePoint += 100;
        
        // Reaction Force 
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse); // 밟았을 때, 플레이어에게도 반발력을 주면 좋음.

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();

        // Sound
        PlaySound("ATTACK");
    }

    void OnDamaged(Vector2 targetPos) // 무적 효과 함수 생성
    {
        // Health Down
        gameManager.HealthDown();

        // Change Layer (Immortal Active)
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7,ForceMode2D.Impulse);
        
        // Animation
        anim.SetTrigger("doDamaged");
        
        Invoke("OffDamaged", 3);

        // Sound
        PlaySound("DAMAGED");
    }

    void OffDamaged() // 무적 해제 함수 생성
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Filp Y
        spriteRenderer.flipY = true;

        // Collider Disable
        capsuleCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Sound
        PlaySound("DIE");
    }
    
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
