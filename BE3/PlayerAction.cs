using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;
    
    float h;
    float v;
    bool isHorizonMove;
    Vector3 dirVec; // 현재 바라보고 있는 방향 값을 가진 변수가 필요
    GameObject scanObject;

    Rigidbody2D rigid;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Value
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // Check Button Down & up
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        // Check Horizontal Move
        // 수평, 수직 이동 버튼이벤트를 변수로 저장
        if (hDown)
            isHorizonMove = true; // 버튼 다운으로 수평이동 체크 // 버튼 업으로도 수평이동 체크
        else if(vDown)
            isHorizonMove = false;
        else if(hUp || vUp)
            isHorizonMove = h != 0; // 현재 AxisRaw 값에 따라 수평, 수직 판단하여 해결

        // Animation
        if (anim.GetInteger("hAxisRaw") != h){
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h); // 서로 타입이 다르면 명시적 형변환으로 처리
        }
        else if (anim.GetInteger("vAxisRaw") != v){
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else
            anim.SetBool("isChange", false);

        // Direction
        if(vDown && v == 1)
            dirVec = Vector3.up;
        else if(vDown && v == -1)
            dirVec = Vector3.down;
        else if(hDown && h == -1)
            dirVec = Vector3.left;
        else if(hDown && h == 1)
            dirVec = Vector3.right;

        // Scan Object
        if(Input.GetButtonDown("Jump") && scanObject != null)
            Debug.Log("This is :" + scanObject.name);
    }

    void FixedUpdate() 
    {
        // Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v); // 플래그 변수 하나로 수평, 수직 이동을 결정
        rigid.velocity = moveVec * Speed;

        // Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object")); // DrawRay로 미리 보고 RayCast를 구현하면 쉬움.

        if(rayHit.collider != null) {
            scanObject = rayHit.collider.gameObject; // RayCast된 오브젝트를 변수로 저장하여 활용!
        }
        else
        {
            scanObject = null;
        }
    }
}
