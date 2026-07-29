//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 메쉬 프로를 사용하기 위해 필요하다.

public class UI_Script : MonoBehaviour
{
    [Header("텀블러 게이지 UI")]
    [SerializeField] private TumblerOfShasha tumblerScript; // 텀블러 샤샤 스크립트
    [SerializeField] private Image tumblerGaugeImage; // 텀블러 게이지
    [SerializeField] private Gradient gaugeColorGradient; // 텀블러 게이지 증가할 때 마다 그라데이션 효과
    [SerializeField] private TextMeshProUGUI tumblerChanceText; // 텀블러 게이지 확률 텍스트
    [SerializeField] private float tumblerGaugeFillSpeed = 2.0f; // 게이지 연출 속도

    private void Awake()
    {
        Detect();
        tumblerGaugeImage.fillAmount = tumblerScript.currentGauge;
    }

    private void Update()
    {
        TumblerGauge_UI();
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

}
