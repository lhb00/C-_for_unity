using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay; // 총알 발사에 대한 Player 스크립트 로직을 복사 + 붙여넣기
    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>(); // Queue : 먼저 입력된 데이터가 먼저 나가는 자료구조(FIFO)
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch() // 따라갈 위치를 계속 갱신해주는 함수 생성
    {
        // Input Pos
        if(!parentPos.Contains(parent.position)) // 부모위치가 가만히 있으면 저장하지 않도록 조건 추가
            parentPos.Enqueue(parent.position); // Enqueue() : 큐에 데이터 저장하는 함수

        // Queue = FIFO (First In First Out)

        // Output Pos
        if (parentPos.Count > followDelay) // 큐에 일정 데이터 개수가 채워지면 그 때부터 반환하도록 작성
            followPos = parentPos.Dequeue(); // Dequeue() : 큐의 첫 데이터를 빼면서 반환하는 함수
        else if (parentPos.Count < followDelay) // 큐가 채워지기 전까진 부모 위치 적용
            followPos = parent.position;
    }

    void Follow() // 움직임 로직을 비우고 함수 이름을 Follow로 변경
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj("bulletFollower");
        bullet.transform.position = transform.position;
        // 위치, 회전 매개변수는 플레이어 transform을 사용
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime; // 딜레이 변수에 Time.deltaTime을 계속 더하여 시간 계산
    }

}