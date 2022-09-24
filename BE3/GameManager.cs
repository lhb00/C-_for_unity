using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용시에는 빼먹지 말기.

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager; // 대화 매니저를 변수로 선언 후, 함수 사용
    public QuestManager questManager; // 퀘스트 매니저를 변수로 생성 후, 퀘스트 번호를 가져옴.
    public Animator talkPanel;
    public Animator portraitAnim;
    public Image portraitImg; // Image UI 접근을 위해 변수 생성 & 할당
    public Sprite prevPortrait; // 과거 스프라이트를 저장해두어 비교 후, 애니메이션 실행
    public TypeEffect talk; // 기존 사용하던 Text 변수를 작성한 이펙트스크립트로 변경
    public Text questText; // 퀘스트 텍스트 UI를 변수로 할당하여 퀘스트 이름 전달
    public GameObject menuSet;
    public GameObject scanObject;
    public GameObject player;
    public bool isAction; // 상태 저장용 변수를 추가
    public int talkIndex;

    void Start()
    {
        GameLoad();
        questText.text = questManager.CheckQuest();
    }

    void Update()
    {
        // Sub Menu
        if (Input.GetButtonDown("Cancel")) // ESC로 켜고 끄기 가능하도록 작성
            SubMenuActive();
    }

    public void SubMenuActive() // Cancel 기능은 함수로 분리하여 호출하도록 작성
    {
        if (menuSet.activeSelf)
        {
            menuSet.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menuSet.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Continue()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void Action(GameObject scanObj)
    {
        // 대화가 모두 끝나야 액션이 끝나도록 설정을 바꾸어야 함
        isAction = true;

        // Get Current Object
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc); // 오브젝트 변수를 매개변수로 활용

        // Visible Talk for Action
        talkPanel.SetBool("isShow", isAction); // SetActive() 함수로 숨기기 & 보여주기를 구현
    }

    void Talk(int id, bool isNpc)
    {
        // Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        if (talk.isAnim) // 매니저에서도 플래그 변수를 이용하여 분기점 로직 생성
        {
            talk.SetMsg("");
            return;
        }

        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex); // 퀘스트번호 + Npc Id = 퀘스트 대화 데이터 Id
        }

        // End Talk
        if(talkData == null) {
            isAction = false;
            talkIndex = 0; // talkIndex는 대화가 끝날 때 0으로 초기화
            questText.text = questManager.CheckQuest(id);
            return; // void함수에서 return은 강제 종료 역할
        }

        // Countinue Talk
        if(isNpc) {
            talk.SetMsg(talkData.Split(':')[0]); // Split(): 구분자를 통하여 배열로 나눠주는 문자열 함수

            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1])); // Parse(): 문자열을 해당 타입으로 변환해주는 함수
            // Parse()는 문자열 내용이 타입과 맞지 않으면 오류 발생!
            portraitImg.color = new Color(1, 1, 1, 1); // NPC 일때만 Image가 보이도록 작성

            // Animation Portrait
            if (prevPortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else {
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }

    public void GameSave() // 매니저에 저장, 불러오기 함수를 생성
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x); // PlayerPrefs: 간단한 데이터 저장 기능을 지원하는 클래스
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y); // 데이터 타입에 맞게 Set 함수 사용
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX")) // 최초 게임 실행했을 땐 데이터가 없으므로 예외처리 로직 작성
            return;

        float x = PlayerPrefs.GetFloat("PlayerX"); // 불러오기 또한 데이터 타입에 맞게 Get 함수 사용
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0); // 불러온 데이터를 게임 오브젝트에 적용
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit() // 매니저에 종료 함수 추가하고 버튼 이벤트와 연결
    {
        Application.Quit(); // Application.Quit()는 에디터에서는 실행되지 않음.
    }
}
