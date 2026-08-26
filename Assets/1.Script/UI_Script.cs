//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 메쉬 프로를 사용하기 위해 필요하다.
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가!

public class UI_Script : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI_Obj;

    [SerializeField] private GameObject OptionUI_Obj;

    //나중에 사운드 전용 스크립트로 옮길수 있으니 여기로 배치
    [Header("배경 음악 연결 요소")]
    [SerializeField] private AudioSource bgmAudioSource; // BGM 스피커
    [SerializeField] private Slider bgmSlider;          // 볼륨 조절 슬라이더 UI

    [Header("점프 사운드 연결 요소")]
    [SerializeField] private AudioSource jumpAudioSource; // 점프 오디오 소스
    [SerializeField] private Slider jumpSlider;          // 볼륨 조절 슬라이더 UI

    [Header("텀블러 사운드 연결 요소")]
    [SerializeField] private AudioSource outburstAudioSource; // 텀블러 오디오 소스
    [SerializeField] private Slider outbursSlider;          // 볼륨 조절 슬라이더 UI

    private bool isPaused = false; // 일시정지 중이라면

    private bool isOption = false;

    private void Awake()
    {
        PauseUI_Obj.SetActive(false);
        OptionUI_Obj.SetActive(false);

        //배경 사운드 관련
        // 게임 시작 시, 현재 BGM의 실제 볼륨 수치(0.0 ~ 1.0)를 슬라이더 위치에 똑같이 맞춘다
        if (bgmAudioSource != null && bgmSlider != null)
        {
            bgmSlider.value = bgmAudioSource.volume;
        }

        if (jumpAudioSource != null && jumpSlider != null)
        {
            jumpSlider.value = jumpAudioSource.volume;
        }

        if (outburstAudioSource != null && outbursSlider != null)
        {
            outbursSlider.value = outburstAudioSource.volume;
        }
    }

    private void Update()
    {
        
        // ESC 키를 누르면 일시정지 상태를 전환한다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();

            if (isOption)
            {
                OptionUI_Obj.SetActive(false);
                isOption = false;
            }
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

    //사운드 옵션 전체 창
    public void ToggleSoundOption()
    {
        if (!isOption) // 사운드 옵션창이 꺼져있고 옵션창이 켜져있다면
        {
            OptionUI_Obj.SetActive(true);
            isOption = true;
        }
        else
        {
            OptionUI_Obj.SetActive(false);
            isOption = false;
        }

    }

   

    // 슬라이더가 움직일 때 실시간으로 호출될 볼륨 조절 함수
    // 매개변수로 float volume을 받는 것이 핵심
    //배경화면 사운드 조절(나중에 따로 스크립트 분리할것인지 고려해둘 것)
    public void BGM_Sound(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume; // 0.0 ~ 1.0 조절
        }
    }

    public void Jump_Sound(float volume)
    {
        if (jumpAudioSource != null)
        {
            jumpAudioSource.volume = volume; // 0.0 ~ 1.0 조절
        }
    }

    public void Outburst_Sound(float volume)
    {
        if (outburstAudioSource != null)
        {
            outburstAudioSource.volume = volume; // 0.0 ~ 1.0 조절
        }
    }

}
