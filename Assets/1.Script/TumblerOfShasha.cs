using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


//샤샤 텀블러의 [돌발행동] 시스템
// 플레이어가 점프를 하다 보면 [돌발행동] 확률이 랜덤으로 증가되며 


public class TumblerOfShasha : MonoBehaviour
{

    [Header("확률 범위 설정")]
    private float currentChance = 0.0f; // 시작 확률 및 현재 확률
    [SerializeField] private float addChanceMin = 0.02f; // 0.05f라고 쓰면 추가될 최소 확률(+5%)라는 의미
    [SerializeField] private float addChanceMax = 0.10f; //  0.15f라고 쓰면 추가될 최대 확률 (+15%)라는 의미

    [Header("돌발행동 힘 배율 범위")] // Multiplier = 증가시킨다라는 뜻
    [SerializeField] private float minOutburstMult = 4f; // 최소 힘 40%
    [SerializeField] private float maxOutburstMult = 20f; // 최대 힘 200%


    public float Outburst()
    {
        if (Random.value < currentChance) //Random.value < 현재 확률
        {
            //돌발행동 발생
            //minOutburstMult와 maxOutburstMult 사이의 임의의 배율을 선택된다.
            float randomMultiplier = Random.Range(minOutburstMult, maxOutburstMult);

            Debug.Log("[돌발행동] 발생 (확률 초기화), 힘 배율 : " + randomMultiplier );
            currentChance = 0.0f; // 발동 후 다시 초기화

            return randomMultiplier; // 돌발 행동이 터졌다고 알려줌
        }
        else
        {
            
            // 돌발행동이 발생하지 않는다면 확률을 높여준다.
            float added = Random.Range(addChanceMin, addChanceMax);
            Debug.Log("증가된 확률" + added * 100f + "%");
            currentChance += added;

            // 확률이 최대 100%(1.0)이 넘지 않도록 안전장치를 걸어준다.
            currentChance = Mathf.Clamp01(currentChance);

            Debug.Log("다음 돌발행동 확률 : " + currentChance * 100f +"%");

            return 1.0f; // 돌발행동이 생기지 않음, 1.0으로 하는 이유는 힘이 추가로 받지 않는 일반 점프임으로 1.0을 곱해 힘 배율이 변화없게끔 설정
        }
    }
}
