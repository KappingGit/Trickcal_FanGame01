using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CutScene : MonoBehaviour
{
    [Header("텀블러 컷씬 UI")]
    /// <summary>
    /// 주의 : Object로 받게끔 설정
    /// </summary>
    [SerializeField] private RectTransform cutScene_UI;

    [SerializeField] private TumblerOfShasha tumblerOfShashaScript;

    //움직이는 컷씬 오브젝트
    [Header("텀블러 약한 물줄기 UI 오브젝트")]
    [SerializeField] private GameObject cutScene01_obj;

    [Header("텀블러 강한 물줄기 UI 오브젝트")]
    [SerializeField] private GameObject cutScene02_obj;

    private Vector2 startPos = new Vector2(-400, -200); // UI 초기 위치

    private Vector2 targetPos = new Vector2(150, -200); // UI 도착 위치

    //private float progress = 0.5f; // 진행도

    private bool isPlaying = false; // 중복 실행 방지용 스위치

    private void Awake()
    {
        // 시작할 때 UI를 화면 밖(초기 위치)으로 미리 세팅해둡니다.
        cutScene_UI.anchoredPosition = startPos;
        cutScene01_obj.SetActive(false);
        cutScene02_obj.SetActive(false);
    }

    private void Update()
    {
        
    }

    // 스크립트가 켜질 때 텀블러의 신호를 연결합니다.
    private void OnEnable()
    {
        if (tumblerOfShashaScript != null)
        {
            tumblerOfShashaScript.OnOutburstTriggered += PlayCutScene;
        }
    }

    // 스크립트가 꺼질 때 신호 연결을 해제합니다 (메모리 관리).
    private void OnDisable()
    {
        if (tumblerOfShashaScript != null)
        {
            tumblerOfShashaScript.OnOutburstTriggered -= PlayCutScene;
            
        }
    }

    // 신호를 받으면 실행되는 함수
    private void PlayCutScene(float multiplier)
    {
        if (!isPlaying) // 이미 재생 중이 아닐 때만 시작!
        {
            StartCoroutine(SlideUI_Sequence(multiplier));
        }
    }
    
    private void CutSceneObj()
    {
       
    }

    IEnumerator SlideUI_Sequence(float multiplier) // 코루틴을 활용
    {
        isPlaying = true; // 재생 중 스위치 ON

        // 조건에 따라 알맞은 UI 오브젝트 활성화
        if (multiplier < 1.0f)
        {
            cutScene01_obj.SetActive(true);  // 약한 물줄기
            cutScene02_obj.SetActive(false);
        }
        else
        {
            cutScene01_obj.SetActive(false);
            cutScene02_obj.SetActive(true);  // 강한 물줄기
        }

        // ==========================================
        // 1단계: 등장 (startPos -> targetPos)
        // ==========================================
        float timeElapsed = 0f;
        float duration = 0.5f; // 0.5초 동안 등장

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            cutScene_UI.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null; // 매 프레임 부드럽게 이동하기 위해 대기
        }
        cutScene_UI.anchoredPosition = targetPos; // 도착지점 쾅! (오차 교정)

        

        // ==========================================
        // 2단계: 중앙에서 2초 동안 머무르기 (대기)
        // ==========================================
        // 여기가 바로 아까 원하셨던 "2초 있다가" 부분입니다. 
        // while문 밖에서 기다려야 UI가 멈춘 상태로 2초가 흘러갑니다.
        yield return new WaitForSeconds(1.5f);
        

        // ==========================================
        // 3단계: 퇴장 (targetPos -> startPos)
        // ==========================================
        timeElapsed = 0f; // 중요: 퇴장하기 전에 타이머를 다시 0으로 초기화!

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            // 시작과 끝 위치를 반대로 뒤집어 줍니다.
            cutScene_UI.anchoredPosition = Vector2.Lerp(targetPos, startPos, t);

            yield return null; // 매 프레임 부드럽게 이동
        }
        cutScene_UI.anchoredPosition = startPos; // 원래 자리로  (오차 교정)

        // 연출 종료 후 UI 오브젝트 끄기 및 스위치 초기화
        cutScene01_obj.SetActive(false);
        cutScene02_obj.SetActive(false);

        isPlaying = false; // 다음 연출을 위해 재생 상태 해제!

        
    }

}
