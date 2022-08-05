using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        Vector3 vec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0); //º¤ÅÍ °ª
        transform.Translate(vec); //3Â÷¿ø º¤ÅÍ
    }

}
