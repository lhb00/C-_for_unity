using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI는 꼭 먼저 using UnityEngine.UI 선언
using UnityEngine.SceneManagement; // 재시작을 위해 SceneManagement 활용
using System.IO; // 파일 읽기를 위한 Sysyem.IO 사용

public class GameManager : MonoBehaviour
{
    public int stage;
    public Animator stageAnim; // Animator 변수를 2개 생성해서 트리거 호출
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints; // 적 프리펨 배열과 생성 위치 배열 변수를 선언

    public float nextSpawnDelay; // 적 생성 딜레이 변수 선p
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList; // 적 출현에 관련된 변수 생성
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" }; // 풀링을 사용하는 GameManager에도 보스 로직 추가
        StageStart();
    }

    public void StageStart()
    {
        // Stage UI Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart"; // 스테이지 숫자가 UI Text에 반영되도록 로직 작성
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear!!";

        // Enemy Spawn File Read
        ReadSpawnFile();

        // Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        // Clear UI Load
        clearAnim.SetTrigger("On");

        // Fade Out
        fadeAnim.SetTrigger("Out");

        // Player Repos
        player.transform.position = playerPos.position;

        // Stage Increament
        stage++;
        if (stage > 2)
            Invoke("GameOver", 6);
        else
            Invoke("StageStart", 5); // 스테이지가 끝나면 다음 스테이지를 시작하도록 함수 호출
    }

    void ReadSpawnFile()
    {
        // 1. 변수 초기화
        spawnList.Clear();
        spawnIndex = 0; // 적 출현에 변수 초기화
        spawnEnd = false;

        // 2, 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset; // TextAsset : 텍스트 파일 에셋 클래스
        // Resources.Load() : Resources 폴더 내 파일 불러오기
        // 스테이지 변수를 통한 적 배치 파일 로드
        StringReader stringReader = new StringReader(textFile.text); // StringReader : 파일 내의 문자열 데이터 읽기 클래스

        while(stringReader != null)
        {
            string line = stringReader.ReadLine(); // ReadLine() : 텍스트 데이터를 한 줄 씩 반환(자동 줄 바꿈)
            Debug.Log(line);

            if (line == null)
                break;

            // 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]); // Split() : 지정한 구분 문자로 문자열을 나누는 함수
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData); // 구조체 변수를 채우고 리스트에 저장
        } // while문으로 텍스트 데이터 끝에 다다를 때까지 반복

        // 텍스트 파일 닫기
        stringReader.Close(); // StringReader로 열어둔 파일은 작업이 끝난 후 꼭 닫기

        // 첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay; // 미리 첫번째 출현 시간을 적용
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd) // 플래그 변수를 적 생성 조건에 추가
        {
            SpawnEnemy();
            // Range() : 정해진 범위 내의 랜덤 숫자 변환 (float, int)
            curSpawnDelay = 0; // 적 생성 후엔 꼭 딜레이 변수 0으로 초기화
        }

        // UI Score Update
        Player playerLogic = player.GetComponent<Player>(); // Update에서 Score Text 업데이트
        scoreText.text = string.Format("{0:n0}", playerLogic.score); // string.format() 지정된 양식으로 문자열을 변환해주는 함수
        // {0:n0} : 세자리마다 쉼표로 나눠주는 숫자 양식
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type) // 기존 적 생성 로직을 구조체를 활용한 로직으로 교체
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]); // 기존의 Instantiate()를 오브젝트 풀링으로 교체
        enemy.transform.position = spawnPoints[enemyPoint].position; // 위치와 각도는 인스턴스 변수에서 적용

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; // 적 생성 직후에 플레이어 변수를 넘겨주는 것으로 프리펩은 이미 Scene에 올라온 오브젝트에 접근 불가능한 문제 해결
        enemyLogic.gameManager = this; // this : 클래스 자신을 일컫는 키워드
        enemyLogic.objectManager = objectManager;
        if(enemyPoint == 5 || enemyPoint == 6) // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);// 속도 방향에 따라 적 비행기 회전 적용
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1); // 생성 위치에 따라 속도를 다르게 설정
        }

        else if(enemyPoint == 7 || enemyPoint == 8) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else // Front Spawn
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
        // 적 비행기 속도를 GameManager가 관리하도록 수정

        // 리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        // 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay; // 적 생성이 완료되면 다음 생성을 위한 시간 갱신
        
    }

    public void UpdateLifeIcon(int life)
    {
        // Image를 일단 모두 투명 상태로 두고, 목숨대로 반투명 설정

        // UI Life Init Disable
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Life Active
        for (int index=0;index<life;index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        // Image를 일단 모두 투명 상태로 두고, 목숨대로 반투명 설정

        // UI Boom Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Boom Active
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer() // 플레이어를 복귀시키는 로직은 매니저가 관리
    {
        Invoke("RespawnPlayerExe", 2f); // 플레이어 복귀는 시간 차를 두기 위해 Invoke() 사용
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false; // bool 변수를 다시 초기화하는 공간도 꼭 구현
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
