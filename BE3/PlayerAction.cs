using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;
    public GameManager manager; // 플레이어에서 매니저 함수를 호출할 수 있게 변수 생성
    
    float h;
    float v;
    bool isHorizonMove;
    Vector3 dirVec; // 현재 바라보고 있는 방향 값을 가진 변수가 필요
    GameObject scanObject;

    // Mobile Key Var
    int up_Value; // 버튼 입력을 받을 변수 12개 생성 (값+Down+Up)x4
    int down_Value;
    int left_Value;
    int right_Value;
    bool up_Down;
    bool down_Down;
    bool left_Down;
    bool right_Down;
    bool up_Up;
    bool down_Up;
    bool left_Up;
    bool right_Up;

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
        // 대화 도중, 플레이어가 이동하여 이탈할 우려가 있음
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value; // 상태 변수를 사용하여 플레이어의 이동을 제한
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;

        // Check Button Down & up
        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down;
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;

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
            manager.Action(scanObject);

        // Mobile Var Init
        // Down, Up 변수는 로직이 끝나면 False로 초기화
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;
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

    public void ButtonDown(string type) // 버튼 이벤트 전용 함수 2개 (Down, Up) 생성
    {
        switch (type) // 4방향을 처리하기 위해 매개변수를 활용한 Switch문 사용
        {
            case "U": // Switch문에서 각 방향마다 변수를 할당
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A": // 버튼 Down 함수에 액션 조건을 추가
                if (scanObject != null)
                    manager.Action(scanObject);
                break;
            case "C":
                manager.SubMenuActive();
                break;
        }

    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Up = true;
                break;
            case "R":
                right_Value = 0;
                right_Up = true;
                break;
        }
    }
}
