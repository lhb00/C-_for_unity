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
    public GameObject grenadeObj;
    public Camera followCamera; // 플레이어에 메인카메라 변수를 만들고 할당하기

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
    bool gDown;
    bool rDown; // 재장전에 관련된 변수와 함수 추가
    bool sDown1; // 장비 단축키 1, 2, 3 따로 변수 생성 및 Input 버튼으로 등록
    bool sDown2;
    bool sDown3;

    bool isJump; // 무한 점프를 막기 위한 제약 조건 필요
    bool isDodge;
    bool isSwap; // 교체 시간차를 위한 플래그 로직 작성
    bool isReload;
    bool isFireReady = true;
    bool isBorder; // 벽 충돌 플래그 bool 변수를 생성
    bool isDamage; // 무적타임을 위해 bool 변수 추가

    Vector3 moveVec;
    Vector3 dogeVec; // 회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가

    Rigidbody rigid; // 물리 효과를 위해 Rigidbody 변수 선언 후, 초기화
    Animator anim;
    MeshRenderer[] meshs; // MeshRenderer 배열 변수 추가

    GameObject nearObject; // 트리거된 아이템을 저장하기 위한 변수 선언
    Weapon equipWeapon; // 기존 지정해둔 현재 장비 변수를 Weapon 타입으로 변경
    int equipWeaponIndex = -1;
    float fireDelay;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); // Animator 변수를 GetComponentINChildren()으로 초기화
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Grenade(); // 수류탄 전용 Input과 함께 함수 생성
        Attack(); // 공격함수 추가
        Reload();
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
        fDown = Input.GetButton("Fire1"); // 공격 인풋을 GetButton()으로 교체
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
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

        if (isSwap || isReload || !isFireReady) // 공격, 장전 중에는 이동 불가 되도록 설정
            moveVec = Vector3.zero;

        if(!isBorder) // 플래그 변수를 이동 제한 조건으로 활용하기
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // transform 이동은 반드시 Time.deltaTime 넣기
        // bool 형태 조건 ? true일 때 값 : false일 때 값(삼항연산자)

        anim.SetBool("isRun", moveVec != Vector3.zero); // SetBool() 함수로 파라미터 값을 설정하기
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        // 1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec); // LookAt() : 지정된 벡터를 향해서 회전시켜주는 함수
        // 2. 마우스에 의한 회전
        if(fDown) // 마우스 클릭했을 때만 회전하도록 조건 추가
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition); // ScreenPointToRay() : 스크린에서 월드로 Ray를 쏘는 함수
            RaycastHit rayHit; // RayCastHit 정보를 저장할 변수 추가
            if (Physics.Raycast(ray, out rayHit, 100)) // out : return처럼 반환값을 주어진 변수에 저장하는 키워드
            {
                Vector3 nextVec = rayHit.point - transform.position; // RayCastHit의 마우스 클릭 위치를 활용하여 회전을 구현
                nextVec.y = 0; // RayCastHit의 높이는 무시하도록 Y축 값을 0으로 초기화
                transform.LookAt(transform.position + nextVec);
            }
        }
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

    void Grenade()
    {
        // 수류탄을 쓰기 이전에 제한 조건들을 작성
        if (hasGrenades == 0)
            return;

        // 마우스 위치로 바로 던질 수 있도록 RayCast 사용
        if(gDown && !isReload && !isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition); // ScreenPointToRay() : 스크린에서 월드로 Ray를 쏘는 함수
            RaycastHit rayHit; // RayCastHit 정보를 저장할 변수 추가
            if (Physics.Raycast(ray, out rayHit, 100)) // out : return처럼 반환값을 주어진 변수에 저장하는 키워드
            {
                Vector3 nextVec = rayHit.point - transform.position; // RayCastHit의 마우스 클릭 위치를 활용하여 회전을 구현
                nextVec.y = 10; // RayCastHit의 높이는 무시하도록 Y축 값을 0으로 초기화

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation); // Instantiate() 함수로 수류탄 생성
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse); // 생성된 수류탄의 리지드바디를 활용하여 던지는 로직 구현
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
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
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot"); // 무기 타입에 따라 다른 트리거 실행
            fireDelay = 0; // 공격딜레이를 0으로 돌려서 다음 공격까지 기다리도록 작성
        }
    }

    void Reload()
    {
        // 무기가 있는지, 무기 타입이 맞는지, 총알은 있는지 확인
        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if(rDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            // 애니메이터 트리거 호출과 플래그변수 변화 작성
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo  + equipWeapon.curAmmo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo - equipWeapon.curAmmo; // 플레이어가 소지한 탄을 고려해서 계산하기
        equipWeapon.curAmmo += reAmmo;
        ammo -= reAmmo;
        isReload = false;
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

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero; // angularVelocity : 물리 회전 속도
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green); // DrawRay() : Scene내에서 Ray를 보여주는 함수
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall")); // Raycast() : Ray를 쏘아 닿는 오브젝트를 감지하는 함수
    }

    void FixedUpdate() // FixedUpdate() 함수와 함께 새로운 함수도 선언하여 호출
    {
        FreezeRotation();
        StopToWall();
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

        else if(other.tag=="EnemyBullet") // OnTriggerEnter에 EnemyBullet의 경우 추가
        {
            if(!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>(); // Bullet 스크립트 재활용하여 데미지 적용
                health -= enemyBullet.damage;
                // 리지드바디 유무를 조건으로 하여 Destroy() 호출
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                StartCoroutine(OnDamage());// 리액션을 위한 코루틴 생성 및 호출
            }

        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        // 반복문을 사용하여 모든 재질의 색상을 변경
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f); // WaitForSeconds()로 무적 타임 조정
        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
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
