using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null; // static의 역할 : 프로그램이 시작되자마자 해당 변수를 메모리에 들고 있게 해준다.
    // static을 쓰면 쓸수록 메모리에 고정적으로 할당되는 양이 많으므로 과부하가 올 수 있다.

    public static DataManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject); // DataManager 오브젝트는 Scene이 전환되더라도 파괴되지 않고 유지됨.
        }

        // 유일성을 위해 이미 존재한다면 파괴
        else
        {
            Destroy(this.gameObject);
        }
    }
    // 저장 기능
    public void Save()
    {
        print("저장 완료!");
    }
}
