//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    [Header("텀블러 게이지 UI")]
    [SerializeField] private TumblerOfShasha tumblerScript; // 텀블러 샤샤 스크립트
    [SerializeField] private Image tumblerGaugeImage; // 텀블러 게이지
    [SerializeField] private Gradient gaugeColorGradient;
    private void Awake()
    {
        if (tumblerScript == null)
        {
            Debug.LogWarning("TumblerOfShasha가 비어있습니다..");
        }

        if (tumblerGaugeImage == null)
        {
            Debug.LogWarning("tumblerGaugeImage 이미지가 비어있습니다..");
        }

    }

    private void Update()
    {
        TumblerGauge_UI();
    }

    private void TumblerGauge_UI()
    {
        // 이미지 소스를 넣으면 fillAmount가 나오며 조절이 가능하다 여기서 Slice로 바꿔주면 된다.
        tumblerGaugeImage.fillAmount = tumblerScript.currentChance;

        // 2. (선택) 확률에 따라 게이지 색상 변경 연출
        tumblerGaugeImage.color = gaugeColorGradient.Evaluate(tumblerScript.currentChance);
    }

}
