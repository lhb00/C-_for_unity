using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 대화 데이터를 저장할 Dictionary 변수 생성
    Dictionary<int, Sprite> portraitData; // 초상화 데이터를 저장할 Dictionary 변수 생성

    public Sprite[] portraitArr;// 초상화 스프라이트를 저장할 배열 생성
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        talkData.Add(1000, new string[] {"안녕?:0", "이 곳에 처음 왔구나?:1", "한 번 둘러보도록 해:0"}); // Add 함수를 사용하여 대화 데이터 입력 추가
        // 대화 하나에는 여러 문장이 들어있으므로 string[] 사용
        // NPC 표정은 문장과 1:1 매칭 // 구분자와 함께 초상화 Index를 문장 뒤에 추가
        talkData.Add(2000, new string[] {"여어.:1", "이 호수는 정말 아름답지?:0", "사실 이 호수에는 무언가의 비밀이 숨겨져 있다고 해.:1"});
        talkData.Add(3000, new string[] {"평범한 나무상자다."});
        talkData.Add(4000, new string[] {"누군가 사용했던 흔적이 있는 책상이다."});

        // Quest Talk
        talkData.Add(10 + 1000, new string[] { "어서 와.:0", "이 마을에 놀라운 전설이 있다는데:1", "오른쪽 호수 쪽에 루도가 알려줄거야.:0" });
        talkData.Add(11 + 2000, new string[] { "여어.:1", "이 호수의 전설을 들으러 온거야?:0", "그럼 일 하나만 해주면 좋을텐데...:1"
                                             , "내 집 근처에 떨어진 동전 좀 주워줬으면 해.:1" });

        talkData.Add(20 + 1000, new string[] { "루도의 동전?:1", "돈을 흘리고 다니면 못쓰지!:3", "나중에 루도에게 한마디 해야겠어.:3" });
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘.:1"});
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다." });
        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워.:2" });

        // 이 상태로는 순서 상관없이 대화가 진행 (무시되는 NPC 발생)

        // 퀘스트 번호 + NPC Id에 해당하는 대화 데이터 작성

        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id)) // ContainsKey() : Dictionary에 key가 존재하는지 검사
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex); // Get First Talk
            else
                return GetTalk(id - id % 10, talkIndex); // Get First Quest Talk
            // 반환 값이 있는 재귀함수는 return까지 꼭 적어주어야 함!
        }

        if (talkIndex == talkData[id].Length) // talkIndex와 대화의 문장 갯수를 비교하여 끝을 확인
            return null;
        else
            return talkData[id][talkIndex]; // id로 대화 Get -> talkIndex로 대화의 한 문장을 Get

    } // 지정된 대화 문장을 반환하는 함수 하나 생성

    public Sprite GetPortrait(int id, int portraitIndex) // 지정된 초상화 스프라이트를 반환할 함수 생성
    {
        return portraitData[id + portraitIndex];
    }

}