// 2. 하나의 에이전트가 사용할 수 있는 FSM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Locations { SweetHome = 0, Library, LectureRoom, PCRoom, Pub};
public class GameController : MonoBehaviour
{
    [SerializeField]
    private string[] arrayStudents; // Student들의 이름 배열
    [SerializeField]
    private GameObject studentPrefab; // Student 타입의 프리펩

    [SerializeField]
    private string[] arrayUnEmployeds;
    [SerializeField]
    private GameObject unEmployedPrefab;

    // 재생 제어를 위한 모든 에이전트 리스트
    private List<BaseGameEntity> entitys;

    public static bool IsGameStop { set; get; } = false;

    private void Awake()
    {
        entitys = new List<BaseGameEntity>();

        for(int i = 0; i < arrayStudents.Length; ++ i)
        {
            // 에이전트 생성, 초기화 메소드 호출
            GameObject clone = Instantiate(studentPrefab);
            Student entity = clone.GetComponent<Student>();
            entity.Setup(arrayStudents[i]);

            // 에이전트들의 재생 제어를 위해 리스트에 저장
            entitys.Add(entity);
        }

        // Unemployed 에이전트 생성
        for(int i = 0; i < arrayUnEmployeds.Length; i++)
        {
            GameObject clone = Instantiate(unEmployedPrefab);
            UnEmployed entity = clone.GetComponent<UnEmployed>();
            entity.Setup(arrayUnEmployeds[i]);

            entitys.Add(entity);
        }

        // 에이전트 데이터베이스 초기화 및 모든 에이전트 등록
        EntityDataBase.Instance.Setup();

        for(int i = 0; i < entitys.Count; ++ i)
        {
            EntityDataBase.Instance.RegisterEntity(entitys[i]);
        }

        // 메시지 관리자 초기화
        MessageDispatcher.Instance.Setup();
    }

    private void Update()
    {
        if (IsGameStop == true) return;

        // 지연 발송되어야 하는 메시지 관리
        MessageDispatcher.Instance.DispatchDelayedMessages();

        // 모든 에이전트의 Updated()를 호출해 에이전트 구동
        for(int i = 0; i < entitys.Count; ++i)
        {
            entitys[i].Updated();
        }
    }

    public static void Stop(BaseGameEntity entity)
    {
        IsGameStop = true;
        entity.PrintText("자넨 야인답게 살았네.");
    }
}
