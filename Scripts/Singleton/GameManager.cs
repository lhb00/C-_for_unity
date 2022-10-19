using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> // 이렇게만 해주면 싱글톤 완성
{
    public void Game()
    {
        print("게임매니저에 싱글톤을 성공적으로 상속하였습니다.");
    }
}
