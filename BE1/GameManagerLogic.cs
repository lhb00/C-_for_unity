using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용 시 UnityEngine.UI 라이브러리 적용 필수

public class GameManagerLogic : MonoBehaviour
{
    public int totalItemCount;
    public int stage;
    public Text stageCountText;
    public Text playerCountText;

    void Awake() {
        stageCountText.text = " / " + totalItemCount;
    }
    public void GetItem(int count) {
        playerCountText.text = count.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            SceneManager.LoadScene(stage);
    }
}
