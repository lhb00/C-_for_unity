using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;
    public int CharPerSeconds; // 글자 재생 속도를 위한 변수 생성(CPS)
    public bool isAnim; // 애니메이션 실행 판단을 위한 플래그 변수 생성

    string targetMsg; // 표시할 대화 문자열을 따로 변수로 저장
    Text msgText;
    AudioSource audioSource; // AudioSource 변수를 생성, 초기화 후 재생 함수에서 Play()

    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg) // 대화 문자열을 받는 함수 생성
    {
        if(isAnim) // 플래그 변수를 이용하여 분기점 로직 작성
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    // 애니메이션 재생을 위한 시작-재생-종료 3개 함수 생성

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);

        // Start Animation
        interval = 1.0f / CharPerSeconds; // 확실한 소수값을 얻기 위해 분자 1.0f 작성
        Debug.Log(interval);

        isAnim = true;

        Invoke("Effecting", interval); // 시간차 반복 호출을 위한 Invoke 함수를 사용
        // 1초/CPS = 1글자가 나오는 딜레이
    }

    void Effecting()
    {
        // End Animation
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        } // 대화 문자열과 Text 내용이 일치하면 종료

        msgText.text += targetMsg[index]; // 문자열도 배열처럼 char값에 접근 가능
        // Sound
        if(targetMsg[index] != ' ' || targetMsg[index] != '.') // 공백과 마침표는 사운드 재생 제외
            audioSource.Play();

        index++;

        // Recursive
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        EndCursor.SetActive(true); // 종료 함수에서는 대화 마침 아이콘을 활성화(시작에선 반대)
    }
}
