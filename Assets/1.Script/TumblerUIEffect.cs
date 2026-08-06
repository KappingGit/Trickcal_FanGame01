using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 메쉬 프로를 사용하기 위해 필요하다.

public class TumblerUIEffect : MonoBehaviour
{
    [Header("텀블러 게이지 UI")]
    [SerializeField] private TumblerOfShasha tumblerScript; // 텀블러 샤샤 스크립트
    [SerializeField] private Image tumblerGaugeImage; // 텀블러 게이지
    [SerializeField] private Gradient gaugeColorGradient; // 텀블러 게이지 증가할 때 마다 그라데이션 효과
    [SerializeField] private TextMeshProUGUI tumblerChanceText; // 텀블러 게이지 확률 텍스트
    [SerializeField] private float tumblerGaugeFillSpeed = 2.0f; // 게이지 연출 속도
    [SerializeField] private Image tumblerImage; // 텀블러 이미지
    [SerializeField] private RectTransform tumblerIcon_RT;
    private float minY = -100f; // 게이지가 0%일 때 텀블러의 Y 좌표(아이콘을 옮겨서 위치 확인후 작성)
    private float maxY = 6f;  // 게이지가 100%일 때 텀블러의 Y 좌표

    private void Awake()
    {
        Detect();
        tumblerGaugeImage.fillAmount = tumblerScript.currentGauge;
    }

    private void Update()
    {
        TumblerGauge_UI();
        TumblerImage();
    }

    private void Detect() // 검출용 함수
    {
        if (tumblerScript == null)
        {
            Debug.LogWarning("TumblerOfShasha가 비어있습니다..");
        }

        if (tumblerGaugeImage == null)
        {
            Debug.LogWarning("tumblerGaugeImage 이미지가 비어있습니다..");
        }

        if (tumblerChanceText == null)
        {
            Debug.LogWarning("tumblerChanceText 텍스트가 비어있습니다..");
        }

    }


    private void TumblerGauge_UI()
    {
        // 이미지 소스를 넣으면 fillAmount가 나오며 조절이 가능하다 여기서 Slice로 바꿔주면 된다.
        //tumblerGaugeImage.fillAmount = tumblerScript.currentChance;

        // 키워드 : Mathf.MoveTowards(현재값, 목표값, 속도)
        // Mathf.MoveTowards는 현재 값(current)에서 목표 값(target)까지 일정한 속도(maxDelta)로 증가시키거나 감소시켜 등속으로 접근하게 만드는 유니티의 수학 함수입니다. 
        tumblerGaugeImage.fillAmount = Mathf.MoveTowards(
                tumblerGaugeImage.fillAmount,   // 출발점 (현재 게이지 위치)
                tumblerScript.currentGauge,                // 도착점 (텀블러의 실제 확률)
                tumblerGaugeFillSpeed * Time.deltaTime   // 속도
            );

        // (선택) 확률에 따라 게이지 색상 변경 연출
        tumblerGaugeImage.color = gaugeColorGradient.Evaluate(tumblerScript.currentGauge);

        // 텀블러 [돌발행동] 확률
        //tumblerChanceText.text = (tumblerScript.currentChance * 100f).ToString("F1") + "%";
        // 숫자가 증가되는 연출을 사용할 거면 아래 사용
        tumblerChanceText.text = (tumblerGaugeImage.fillAmount * 100f).ToString("F1") + "%";
    }

    private void TumblerImage()
    {
        // 1. 게이지의 현재 비율을 가져옵니다. (0.0 ~ 1.0)
        float currentRatio = tumblerGaugeImage.fillAmount;

        // 2. [Mathf.Lerp] 비율(0~1)에 맞게 현재 Y 높이를 계산합니다.
        // currentRatio가 0이면 minY, 1이면 maxY, 0.5면 딱 중간값을 반환합니다.
        float targetY = Mathf.Lerp(minY, maxY, currentRatio);

        // 3. [RectTransform & Vector2] 
        // 기존 X좌표는 그대로 유지하고, Y좌표만 방금 계산한 targetY로 바꿔서 적용합니다.
        tumblerIcon_RT.anchoredPosition = new Vector2(tumblerIcon_RT.anchoredPosition.x, targetY);
    }

}
