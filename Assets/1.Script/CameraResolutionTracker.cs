using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionTracker : MonoBehaviour
{
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
}
