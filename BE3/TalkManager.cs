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
        talkData.Add(1000, new string[] {"안녕?:0", "이 곳에 처음 왔구나?:1"}); // Add 함수를 사용하여 대화 데이터 입력 추가
        // 대화 하나에는 여러 문장이 들어있으므로 string[] 사용
        // NPC 표정은 문장과 1:1 매칭 // 구분자와 함께 초상화 Index를 문장 뒤에 추가
        talkData.Add(2000, new string[] {"여어.:1", "이 호수는 정말 아름답지?:0", "사실 이 호수에는 무언가의 비밀이 숨겨져 있다고 해.:1"});
        talkData.Add(100, new string[] {"평범한 나무상자다."});
        talkData.Add(200, new string[] {"누군가 사용했던 흔적이 있는 책상이다."});

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
        if(talkIndex == talkData[id].Length) // talkIndex와 대화의 문장 갯수를 비교하여 끝을 확인
            return null;
        else
            return talkData[id][talkIndex]; // id로 대화 Get -> talkIndex로 대화의 한 문장을 Get
    } // 지정된 대화 문장을 반환하는 함수 하나 생성

    public Sprite GetPortrait(int id, int portraitIndex) // 지정된 초상화 스프라이트를 반환할 함수 생성
    {
        return portraitData[id + portraitIndex];
    }
}
