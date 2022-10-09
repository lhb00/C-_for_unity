using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    // UI, 애니메이터, 플레이어를 담을 변수 생성
    public RectTransform uiGroup;
    public Animator anim;

    // 각 버튼에 따른 아이템 프리펩, 가격, 위치 배열 변수 생성
    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText; // 금액 부족을 알려주기 위해서 대사 텍스트도 변수에 저장

    Player enterPlayer;

    // 입장 Enter, 퇴장 Exit 함수 생성

    public void Enter(Player player)
    {
        enterPlayer = player; // 입장 시, 플레이어 정보를 저장하면서 UI 위치 이동
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000; // 퇴장 시, 애니메이션 실행하면서 UI 위치 이동
    }

    public void Buy(int index) // 구입 Buy 함수 추가
    {
        int price = itemPrice[index];
        // 금액이 부족하면 return으로 구입로직 건너뛰기
        if(price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
                         + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);// 구입 성공 시, Instantiate()로 아이템 생성
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1]; // 코루틴으로 금액 부족 대사 몇초간 띄우기
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
