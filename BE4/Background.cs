using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2; // orthographicSize : orthographic 카메라 Size
    }

    void Update()
    {
        Move();
        Scrolling();
    }

    void Move()
    {
        Vector3 curPos = transform.position; // transform을 이용한 이동 구현
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling()
    {

        if (sprites[endIndex].position.y < viewHeight * (-1))
        {
            // Sprite Reuse
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * 10;
            // EndIndex 스프라이트를 StartIndex 뒤로 이동

            // Cursor Indexs Change
            // 이동이 완료되면 EndIndex, StartIndex 갱신
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1; // 후위 연산자는 해당 라인이 끝나야 변수에 연산 적용
            // 배열을 넘어가지 않도록 예외 처리
        }
    }

}
