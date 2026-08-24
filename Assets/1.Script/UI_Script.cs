//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 메쉬 프로를 사용하기 위해 필요하다.
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가!

public class UI_Script : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI_Obj;

    private bool isPaused = false; // 일시정지 중이라면

    private void Awake()
    {
        PauseUI_Obj.SetActive(false);
    }

    private void Update()
    {
        
        // ESC 키를 누르면 일시정지 상태를 전환한다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Time.timeScale == 0f) return; // 이거의 위치에 따라 esc기능이 조금 달라짐
    }

    
    public void TogglePause()
    {
        if (!isPaused)
        {
            //일시정지
            PauseUI_Obj.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            PauseUI_Obj.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    // '메인 메뉴로 나가기' 버튼에 연결할 함수
    public void GoToMainMenu()
    {
        // 중요: 씬을 넘어가기 전에 반드시 시간을 정상화해야 함!
        Time.timeScale = 1f;

        // "MainMenu"라는 이름의 씬으로 이동 (실제 씬 이름에 맞게 수정)
        SceneManager.LoadScene("MainScene");
    }
}
