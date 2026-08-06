using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaShaMainCamera : MonoBehaviour
{
    [Header("플레이어 연결")]
    [SerializeField] private Transform playerTrans;

    [Header("카메라 설정")]
    [SerializeField] private float cameraHeight = 10f; // 카메라의 세로 높이 (Size 5 기준 높이는 size * 5 = 10)
    [SerializeField] private float fixed_X = 0f;        // 카메라의 고정된 X 위치 (맵 중앙)
    [SerializeField] private float bottomLimit_Y = 0f;  // 카메라가 내려갈 수 있는 최하단 Y 위치

    private float currentCam_Y; // 현재 카메라가 위치해야 할 Y 좌표

    private void Awake()
    {
        
    }

    private void Update()
    {
        
        if (playerTrans == null) return;

        // 1. 플레이어가 현재 화면의 '맨 위'를 뚫고 올라갔을 때
        if (playerTrans.position.y > currentCam_Y + (cameraHeight / 2f)) // 플레이어y > 현재캠 Y축 +(카메라 높이 / 2)
        {
            // 카메라 목표 위치를 한 층(cameraHeight) 올림
            currentCam_Y += cameraHeight; // 현재 캠의 높이를 한층 높인다.
            Camera();
        }
        // 2. 플레이어가 현재 화면의 '맨 아래'로 떨어졌을 때
        else if (playerTrans.position.y < currentCam_Y - (cameraHeight / 2f)) // 플레이어y < 현재캠 Y축 +(카메라 높이 / 2)라면
        {
            // 단, 최하단(bottomLimitY)보다는 밑으로 내려가지 않게 방어
            if (currentCam_Y > bottomLimit_Y)
            {
                currentCam_Y -= cameraHeight; // 현재 캠의 높이를 한층 낮춘다.
                Camera();
            }
        }
    }

    private void Camera()
    {
        // X축은 고정값, Y축은 계산된 층수, Z축은 카메라 기본값(-10) 유지
        transform.position = new Vector3(fixed_X, currentCam_Y, -10f);
    }

}
