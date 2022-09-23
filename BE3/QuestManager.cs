using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex; // 퀘스트 대화순서 변수 생성
    public GameObject[] questObject; // 퀘스트 오브젝트를 저장할 변수 생

    Dictionary<int, QuestData> questList; // 퀘스트 데이터를 저장할 Dictionary 변수 생성 

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기."
                      , new int[] {1000, 2000})); // Add 함수로 <QuestId, QuestData> 데이터를 저장
        // int[]에는 해당 퀘스트에 연관된 NPC Id를 입력
        questList.Add(20, new QuestData("루도의 동전 찾아주기."
                      , new int[] { 5000, 2000 }));

        questList.Add(30, new QuestData("퀘스트 올 클리어!"
                      , new int[] { 0 }));
    }


    public int GetQuestTalkIndex(int id) // Npc Id를 받고 퀘스트번호를 반환하는 함수 생성
    {
        return questId + questActionIndex; // 퀘스트 번호 + 퀘스트 대화순서 = 퀘스트 대화 Id
    }

    public string CheckQuest(int id) // 대화 진행을 위해 퀘스트 대화순서를 올리는 함수 생성
    // 퀘스트 이름을 반환하도록 개조
    { 
        // Next Talk Target
        if (id == questList[questId].npcId[questActionIndex]) // 순서에 맞게 대화했을 때만 퀘스트 대화순서를 올리도록 작성.
            questActionIndex++;

        // Control quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length) // 퀘스트 대화순서가 끝에 도달했을 때 퀘스트번호 증가
            NextQuest();

        // Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest() // 함수 오버로딩 했음. 마소가 만든 언어는 다 비슷하구나.
    {
        // Quest Name
        return questList[questId].questName;
    }

    void NextQuest() // 다음 퀘스트를 위한 함수 생성
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject() // 퀘스트 오브젝트를 관리할 함수 생성
    {
        switch(questId)
        {
            case 10:
                if (questActionIndex == 2) // 퀘스트 번호, 퀘스트 대화순서에 따라 오브젝트 조절
                    questObject[0].SetActive(true);
                break;

            case 20:
                if (questActionIndex == 0)
                    questObject[0].SetActive(true); // 불러오기 했을 당시의 퀘스트 순서와 연결된 오브젝트 관리 추가 

                else if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }
}
