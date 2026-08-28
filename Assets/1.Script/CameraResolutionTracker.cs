using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionTracker : MonoBehaviour
{
    //기존 방식 임시로 주석처리
    /*
     private void Awake()
    {
        // 1. 카메라 컴포넌트 가져오기
        Camera cam = GetComponent<Camera>();

        // 2. 우리가 만든 게임의 목표 비율 (예: 16:9)
        // 만약 세로형 게임이라면 9f / 16f 로 변경하세요!
        float targetRatio = 16f / 9f;

        // 3. 현재 게임을 실행한 기기의 화면 비율
        float currentRatio = (float)Screen.width / Screen.height;

        // 4. 비율 차이 계산
        float scaleHeight = currentRatio / targetRatio;

        // 카메라의 렌더링 영역(Viewport Rect) 정보를 가져옵니다.
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // 기기 화면이 목표보다 '세로'로 더 길 때 (위아래 검은 띠 생성)
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // 기기 화면이 목표보다 '가로'로 더 넓을 때 (좌우 검은 띠 생성 - 맵 밖이 보이는 문제 해결!)
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleWidth) / 2.0f;
        }

        // 5. 계산된 영역을 카메라에 최종 적용
        cam.rect = rect;
    }
     */


    private Camera cam;

    // 이전 프레임의 창 크기를 기억해둘 변수
    private int lastWidth;
    private int lastHeight;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        // 시작할 때 한 번 비율 맞추기
        UpdateResolution();
    }

    private void Update()
    {
        // 최적화: 매 프레임 계산하면 무거우니, 창 크기가 변했을 때만 다시 계산한다
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateResolution();
        }
    }

    // 화면 비율을 계산하고 레터박스를 씌우는 함수
    private void UpdateResolution()
    {
        // 현재 창 크기를 '과거 창 크기'로 갱신하여 기억해둡니다.
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float targetRatio = 16f / 9f; // 목표 비율 (16:9)
        float currentRatio = (float)Screen.width / Screen.height; // 현재 창 비율
        float scaleHeight = currentRatio / targetRatio;

        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // 창이 위아래로 길쭉할 때 (위아래 검은 띠)
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            // 가로 설정은 원래대로 초기화 (이전 변경값 지우기)
            rect.width = 1.0f;
            rect.x = 0f;
        }
        else
        {
            // 창이 양옆으로 길쭉할 때 (좌우 검은 띠)
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleWidth) / 2.0f;

            // 세로 설정은 원래대로 초기화 (이전 변경값 지우기)
            rect.height = 1.0f;
            rect.y = 0f;
        }

        // 카메라에 씌우기
        cam.rect = rect;
    }
}
