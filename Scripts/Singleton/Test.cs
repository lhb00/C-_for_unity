using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Save(); // 싱글톤 구조가 아니었다면 DataManager를 찾는데 복잡한 과정이 있었을 것이다.
        GameManager.Instance.Game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
