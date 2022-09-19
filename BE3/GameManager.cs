using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용시에는 빼먹지 말기.

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager; // 대화 매니저를 변수로 선언 후, 함수 사용
    public GameObject talkPanel;
    public Image portraitImg; // Image UI 접근을 위해 변수 생성 & 할당
    public Text talkText;
    public GameObject scanObject;
    public bool isAction; // 상태 저장용 변수를 추가
    public int talkIndex;
    
    public void Action(GameObject scanObj)
    {
        // 대화가 모두 끝나야 액션이 끝나도록 설정을 바꾸어야 함
        isAction = true;
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc); // 오브젝트 변수를 매개변수로 활용

        talkPanel.SetActive(isAction); // SetActive() 함수로 숨기기 & 보여주기를 구현
    }

    void Talk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if(talkData == null) {
            isAction = false;
            talkIndex = 0; // talkIndex는 대화가 끝날 때 0으로 초기화
            return; // void함수에서 return은 강제 종료 역할
        }
        if(isNpc) {
            talkText.text = talkData.Split(':')[0]; // Split(): 구분자를 통하여 배열로 나눠주는 문자열 함수

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1])); // Parse(): 문자열을 해당 타입으로 변환해주는 함수
            // Parse()는 문자열 내용이 타입과 맞지 않으면 오류 발생!
            portraitImg.color = new Color(1, 1, 1, 1); // NPC 일때만 Image가 보이도록 작성
        }
        else {
            talkText.text = talkData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }
}
