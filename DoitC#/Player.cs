using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public AudioSource JumpSound;
    public AudioSource HitSound;
    public AudioSource CoinSound;

    float hAxis;
    float vAxis;
    bool jDown;

    bool isJump;
    bool isBorder;

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;


        anim.SetBool("isWalk", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);

        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

            JumpSound.Play();
        }
    }

    void FixedUpdate()
    {
        StopToWall();
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green); // DrawRay() : Scene내에서 Ray를 보여주는 함수
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall")); // Raycast() : Ray를 쏘아 닿는 오브젝트를 감지하는 함수
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
            HitSound.Play();

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            CoinSound.Play();
            Destroy(other.gameObject);
        }
    }
}
