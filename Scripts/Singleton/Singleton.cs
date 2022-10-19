using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각각의 매니저들을 일일이 싱글톤으로 만드는 것이 귀찮으므로 제네릭을 사용
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(transform.parent != null && transform.root != null) // 부모/최상위 오브젝트가 있으면
        {
            DontDestroyOnLoad(this.transform.root.gameObject); // 그 오브젝트를 파괴하면 안됨
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
