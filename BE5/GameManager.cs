using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 클래스 사용을 위해 UnityEngine.UI 네임스페이스 호출
using UnityEngine.SceneManagement; // Scene 관련 함수를 사용하려면 SceneManagement 필요

public class GameManager : MonoBehaviour
{
    // 필요한 모든 변수를 public으로 선언
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    // 각 종류별 소환된 몬스터 숫자 변수 선언
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    // 몬스터 리스폰에 필요한 변수들 선언
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    // 게임오버 판넬, 최종 점수, 최고점수확인 변수 추가
    public GameObject overPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreText;
    public Text bestText;

    void Awake()
    {
        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore")); // PlayerPrefs에서 저장된 데이터를 불러오기
        // string.Format()함수로 문자열 양식 적용

        if (PlayerPrefs.HasKey("MaxScore")) // HasKey() 함수로 Key 있는지 확인 후, 없다면 0으로 저장
            PlayerPrefs.SetInt("MaxScore", 0);
    }

    public void GameStart() // 게임시작 버튼을 위한 함수 생성
    {
        menuCam.SetActive(false); // 메뉴 관련 오브젝트는 비활성화, 게임 관련 오브젝트는 활성화
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver() // 게임오버 시, 게임 판넬은 비활성화하고 게임오버 판넬 활성화
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");

        // 기존에 저장된 최고점수를 불러와 비교 후 높으면 갱신
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart() // 게임 오버 후, 돌아가기 위해 재시작 함수 생성
    {
        SceneManager.LoadScene(0); // LoadScene() 함수로 장면을 다시 불러 재시작 구현
    }

    // 스테이지 시작, 종료 함수 생성 

    public void StageStart()
    {
        // 상점과 스테이지 시작존은 상태에 따라 활성/비활성화
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach (Transform zone in enemyZones) // 소환 존은 스테이지 시작할 땐 활성화, 종료되면 비활성화
            zone.gameObject.SetActive(true);

        isBattle = true; // 전투 상태임을 알 수 있도록 bool 변수 컨트롤
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = new Vector3(10, 1.42f, 10); // 스테이지 종료 시 플레이어 원 위치
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++; // 스테이지 종료 시 stage값 1 증가
    }

    // 코루틴으로 전투 상태를 구현
    IEnumerator InBattle()
    {
        // 일정 스테이지 마다 보스가 소환 되도록 조건 추가
        if(stage % 5 == 0)
        {
            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.manager = this;
            enemy.target = player.transform;
            boss = instantEnemy.GetComponent<Boss>();
        }

        else
        {
            // 소환 리스트를 for문을 사용하여 데이터 채우기
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                // 소환 리스트 데이터 만들 때 숫자까지 계산
                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                }
            }

            // while문으로 지속적인 몬스터 소환
            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation); // 생성 시, 소환 리스트의 첫번째 데이터를 사용
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0); // 생성 후 사용된 데이터는 RemoveAt() 함수로 삭제
                yield return new WaitForSeconds(4f); // 안전하게 while문을 돌리기 위해선 꼭 yield return 포함
            }

        }

        // 남은 몬스터 숫자를 검사하는 while문 추가
        while(enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime; // 플레이 타임은 델타타임을 사용하여 증가
    }

    void LateUpdate() // LateUpdate() : Update()가 끝난 후 호출되는 생명주기
    {
        // 상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;

        // 초단위 시간을 3600, 60으로 나누어 시분초로 계산
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int sec = (int)playTime % 60;

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);

        // 플레이어 UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        // 현재 무기 상태에 따라 탄약 표시
        if (player.equipWeapon == null)
            playerAmmoTxt.text = "- / " + player.ammo;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "- / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        // 무기 UI
        // 무기 아이콘은 보유 상황에 따라 알파값만 변경
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 몬스터 숫자 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        // 보스 체력 UI
        if(boss != null) // 보스 변수가 비어있을 때 UI 업데이트 하지 않도록 조건 추가
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            if (boss.curHealth <= 0)
                bossHealthBar.localScale = new Vector3(0, 1, 1);
            else
                bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1); // 보스 체력 이미지의 Scale을 남은 체력 비율에 따라 변경                                                                                   // int 형태끼리 연산하면 결과값도 int이므로 주의
        }

        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200;
        }

    }
}
