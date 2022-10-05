using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision) // OnCollisionEnter()에서 각각 충돌 로직 작성
    {
        if(collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }

        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
