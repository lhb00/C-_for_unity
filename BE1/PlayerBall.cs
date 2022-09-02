using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jumpPower;
    public int itemCount;
    public GameManagerLogic manager;
    bool isJump;
    Rigidbody rigid;
    AudioSource audio;
    void Awake() 
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update() 
    {
        if(Input.GetButtonDown("Jump") && !isJump) {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }

    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor") {
            isJump = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") {
            itemCount++;
            audio.Play();
            other.gameObject.SetActive(false); // SetActive(bool): 오브젝트 활성화 함수
            manager.GetItem(itemCount);
        }
        else if(other.tag == "Point") {
            // Find 계열 함수는 부하를 초래할 수 있으므로 피하는 것이 좋다.
            if(itemCount == manager.totalItemCount) {
                // Game Clear!
                if(manager.stage==2)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene(manager.stage + 1);
            }
            else {
                // Restart
                SceneManager.LoadScene(manager.stage); // SceneManager: 장면을 관리하는 기본 클래스 // LoadScene(): 주어진 장면을 불러오는 함수
            }
        }
    }
}
