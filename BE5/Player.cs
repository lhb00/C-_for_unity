using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 인스펙터 창에서 설정할 수 있도록 public 변수 추가
    public GameObject[] weapons; // 플레이어의 무기관련 배열 함수 2개 선언
    public bool[] hasWeapons;
    public GameObject[] grenades; // 공전하는 물체를 컨트롤하기 위해 배열변수 생성
    public int hasGrenades;

    public int ammo; // 탄약, 동전, 체력, 수류탄(필살기) 변수 생성
    public int health;
    public int coin;


    public int maxAmmo; // 각 수치의 최대값을 저장할 변수도 생성
    public int maxHealth;
    public int maxCoin;
    public int maxHasGrenades;

    float hAxis; // Input Axis 값을 받을 전역변수 선언
    float vAxis;
    bool wDown;
    bool jDown; // bool 변수 선언 후, GetButtonDown()으로 점프 입력 받기
    bool iDown;
    bool fDown; // 키입력, 공격딜레이, 공격준비 변수 선언
    bool sDown1; // 장비 단축키 1, 2, 3 따로 변수 생성 및 Input 버튼으로 등록
    bool sDown2;
    bool sDown3;

    bool isJump; // 무한 점프를 막기 위한 제약 조건 필요
    bool isDodge;
    bool isSwap; // 교체 시간차를 위한 플래그 로직 작성
    bool isFireReady = true;

    Vector3 moveVec;
    Vector3 dogeVec; // 회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가

    Rigidbody rigid; // 물리 효과를 위해 Rigidbody 변수 선언 후, 초기화
    Animator anim;

    GameObject nearObject; // 트리거된 아이템을 저장하기 위한 변수 선언
    Weapon equipWeapon; // 기존 지정해둔 현재 장비 변수를 Weapon 타입으로 변경
    int equipWeaponIndex = -1;
    float fireDelay;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); // Animator 변수를 GetComponentINChildren()으로 초기화
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack(); // 공격함수 추가
        Dodge();
        Interaction();
        Swap();
    }

    // 코드를 기능에 따라 구분되게 함수로 분리하기

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); // GetAxisRaw() : Axis 값을 정수로 반환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk"); // Shift는 누를 때만 작동되도록 GetButton() 함수 사용
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // normalized : 방향 값이 1로 보정된 벡터

        if (isDodge) // 회피 중에는 움직임 벡터 -> 회피방향 벡터로 바뀌도록 구현
            moveVec = dogeVec;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // transform 이동은 반드시 Time.deltaTime 넣기
        // bool 형태 조건 ? true일 때 값 : false일 때 값(삼항연산자)

        if (isSwap || !isFireReady) // 공격 중에는 이동 불가 되도록 설정
            moveVec = Vector3.zero;

        anim.SetBool("isRun", moveVec != Vector3.zero); // SetBool() 함수로 파라미터 값을 설정하기
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); // LookAt() : 지정된 벡터를 향해서 회전시켜주는 함수
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap) // bool 변수는 실행 조건으로 활용 // bool 값을 반대로 사용하고 싶다면 앞에 ! 추가 // 액션 도중에 다른 액션이 실행되지 않도록 조건 추가
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse); // AddForce() 함수로 물리적인 힘을 가하기
            anim.SetBool("isJump",true); // 기존 코드를 활용하여 애니메이션 로직 작성
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Attack()
    {
        if (equipWeapon == null) // 무기가 있을 때만 실행되도록 현재장비 체크
            return;
        fireDelay += Time.deltaTime; // 공격딜레이에 시간을 더해주고 공격가능 여부를 확인
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use(); // 조건이 충족되면 무기에 있는 함수 실행
            anim.SetTrigger("doSwing");
            fireDelay = 0; // 공격딜레이를 0으로 돌려서 다음 공격까지 기다리도록 작성
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero &&!isJump && !isDodge && !isSwap) // 움직임을 조건으로 추가해서 점프와 회피로 나누기
        {
            dogeVec = moveVec;
            speed *= 2; // 회피는 이동속도만 2배 상승하도록 작성
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f); // Invoke() 함수로 시간차 함수 호출
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Interaction() 
    {
        if(iDown && nearObject != null && !isJump && !isDodge) // 상호작용 함수가 작동될 수 있는 조건 작성
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true; // 아이템 정보를 가져와서 해당 무기 입수를 체크

                Destroy(nearObject);
            }
        }
    }

    void Swap() // 무기 교체 함수 생성
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) // 무기 중복 교체, 없는 무기 확인을 위한 조건 추가
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1)) // 무기 중복 교체, 없는 무기 확인을 위한 조건 추가
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2)) // 무기 중복 교체, 없는 무기 확인을 위한 조건 추가
            return;

        int weaponIndex = -1; // 단축키에 맞는 무기를 배열에서 활성화하기
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge) // 단축기 셋 중 하나만 눌러도 되도록 OR 조건 작성
        {
            if(equipWeapon != null) // 빈손일 경우는 생각하여 조건 추가하기
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void OnCollisionEnter(Collision collision) // OnCollisionEnter() 이벤트 함수로 착지 구현
    {
        if(collision.gameObject.tag == "Floor") // 태그를 활용하여 바닥에 대해서만 작동하도록
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other) // OnTriggerEnter()에서 트리거 이벤트 작성
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type) // enum 타입변수 + switch문으로 간단명료하게 조건문 생성
            {
                // enum 타입에 맞게 아이템 수치를 플레이어 수치에 적용하기
                // 플레이어 수치가 최대값을 넘지 않도록 로직 추가
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true); // 수류탄 개수대로 공전체가 활성화 되도록 구현
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other) // 트리거 이벤트인 OnTriggerStay, Exit 사용 
    {
        if (other.tag == "Weapon") // Weapon 태그를 조건으로 하여 로직 작성
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }


}
