using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 매니저는 점수와 스테이지를 관리
public class GameManager : MonoBehaviour
{
    public int totalPoint; // 점수와 스테이지 전역 변수 생성
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth; // UI를 담을 변수들을 생성(이미지는 배열)
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;

    void Update() // 점수는 update 문으로 표시
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        // Change Stage
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true); // StageIndex에 따라 스테이지 활성화/비활성화
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1); // stageIndex는 말 그대로 인덱스 값이라 보여줄 땐 +1
        }
        else // Game Clear
        {
            // Player Control Lock
            Time.timeScale = 0; // 완주하게 되면 timeScale = 0으로 시간을 멈춰둠
            // Result UI
            Debug.Log("게임 클리어!");
            // Restart Button UI
            UIRestartBtn.SetActive(true);
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>(); // 버튼 텍스트는 자식오브젝트이므로 InChildren을 붙여주어야함.
            btnText.text = "Clear!";
            UIRestartBtn.SetActive(true);
        }

        // Calculate Point
        totalPoint += stagePoint; // 스테이지가 올라가면 totalPoint에 누적
        stagePoint = 0;
    }

    public void HealthDown()
    
    {
        if(health > 1) {// 체력이 0이 되면 죽음 함수를 호출
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f); // 체력은 health 값으로 해당 이미지 색상을 어둡게 변경
        }
        else {
            // Player Die Effect
            player.OnDie();
            // Result UI
            Debug.Log("죽었습니다!");

            // Retry Button UI
            UIRestartBtn.SetActive(true); // 재시작 버튼은 게임이 끝났을 때 활성화
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            // Player Reposition
            if(health > 1){
                PlayerReposition();
            }
            
            // Health Down
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-9, 2.5f, -1); // 낭떠러지 로직에 플레이어 원위치 기능도 추가
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
