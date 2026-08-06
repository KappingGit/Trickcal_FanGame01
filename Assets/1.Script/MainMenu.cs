using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;// 씬 이동에 필요하다

public class MainMenu : MonoBehaviour
{
    
    public void StartButton() // 게임 시작 버튼 public으로 하기
    {
        SceneManager.LoadScene("GameScene01");
    }

    public void ExitButton() // 게임 종료 버튼
    {
        Application.Quit();
    }
}
