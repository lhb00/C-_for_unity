using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용시에는 빼먹지 말기.

public class GameManager : MonoBehaviour
{
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;
    public bool isAction; // 상태 저장용 변수를 추가
    
    public void Action(GameObject scanObj)
    {
        if(isAction) { // Exit Action
            isAction = false;
        }

        else { // Enter Action
            isAction = true;
            scanObject = scanObj;
            talkText.text = "이것의 이름은 " + scanObject.name + "이라고 한다.";
        }
        talkPanel.SetActive(isAction); // SetActive() 함수로 숨기기 & 보여주기를 구현
    }
}
