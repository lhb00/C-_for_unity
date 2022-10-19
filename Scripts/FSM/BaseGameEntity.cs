using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    // static 변수이므로 1개만 존재
    private static int m_iNextValidID = 0;

    // BaseGameEntity를 상속받는 모든 게임 오브젝트는 ID 번호를 부여받음
    // 0부터 시작하여 1씩 증가

    private int id;
    public int ID
    {
        set
        {
            id = value;
            m_iNextValidID++;
        }

        get => id;
    }

    private string entityName; // 에이전트 이름
    private string personalColor; // 에이전트 색상(텍스트 출력용)

    public string EntityName => entityName;

    // 파생 클래스에서 base.Setup()으로 호출
    public virtual void Setup(string name)
    {
        // 고유 번호 설정
        ID = m_iNextValidID;

        // 이름 설정
        entityName = name;

        // 고유 색상 설정
        int color = Random.Range(0, 1000000);
        personalColor = $"#{color.ToString("X6")}";
    }

    // GameController 클래스에서 모든 에이전트의 Updated()를 호출해 에이전트 구동
    public abstract void Updated();

    public abstract bool HandleMessage(Telegram telegram);

    public void PrintText(string text)
    {
        Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
    }
}
