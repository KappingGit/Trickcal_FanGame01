using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System; //이걸 사용함으로 Random와 라이브러이 충돌이 생겨서 아래처럼 바꿔준다. 이래야 Random에 문제가 안생긴다.
using Random = UnityEngine.Random;  // "이 스크립트에서 Random은 유니티 Random을 의미한다!" 이거 엄청 중요!!!!!!!
//using JetBrains.Annotations;

//샤샤 텀블러의 [돌발행동] 시스템
// 플레이어가 점프를 하다 보면 [돌발행동] 확률이 랜덤으로 증가되며 100%에 도달하게 되면 작동된다. 
public class TumblerOfShasha : MonoBehaviour
{

    //[Header("확률 범위 설정")]

    [HideInInspector] public float currentGauge = 0.0f; // 시작 확률 및 현재 확률, public으로 해서 UI에 사용하기

    [Header("게이지 % 범위 설정 : n%")]
    [SerializeField] private float addGaugeMin = 2f; // 5라고 쓰면 추가될 최소 확률(+5%)라는 의미
    [SerializeField] private float addGaugeMax = 15f; //  15라고 쓰면 추가될 최대 확률 (+15%)라는 의미

    [Header("돌발행동 힘 배율 범위 : n배")] // Multiplier = 증가시킨다라는 뜻
    [SerializeField] private float minOutburstMult = 0.8f; // 최소 힘 1f = 1배
    [SerializeField] private float maxOutburstMult = 2f; // 최대 힘 2f = 2배

    //---------------------------------------------------------
    [Header("텀블러의 돌발행동 사운드 설정")]
    [SerializeField][Tooltip("플레이어의 AudioSource 컴포넌트 넣기")] private AudioSource sfxAudioSource; // 스피커 컴포넌트 (동일한 스피커 써도 됨)
    [SerializeField][Tooltip("돌발행동 관련 음성 집어넣기")]private AudioClip outburstSound;        // 돌발행동 소리 파일
    //---------------------------------------------------------

    /// <summary>
    /// [돌발행동]을 한다는 것을 알기위한 용도(가독성)
    /// </summary>
    //public bool IsOutburst { get; private set; }// 프로퍼티는 외부에서 IsOutburst를 읽을순 있지만 값을 바꾸는 행위는 "여기" 스크립트에서 밖에 못한다.

    /// <summary>
    /// 이벤트 시스템을 활용해본다.(가독성을 위해) 끄아아아아아
    /// </summary>
    public event Action<float> OnOutburstTriggered; // 돌발행동이 터졌음을 외부에 알리는 신호(이벤트) 선언

    public float Outburst()
    {
        // 엄청 중요!!! 인스펙터의 퍼센트(%) 수치를 계산용 수치(0.0~1.0)로 변환 (가독성을 위함)
        float addMin = addGaugeMin / 100f; // 2% -> 0.02
        float addMax = addGaugeMax / 100f; // 10% -> 0.10


        #region [사용안함] (변경전) 점프할 때마다 [돌발행동]확률이 증가하게 되는 시스템
        //// 중요!!! 키워드 : Random.value는 유니티에서 유니티가 제공하는 주사위 무조건 0.0에서 1.0 사이의 숫자를 랜덤으로 뽑는다.
        //if (Random.value < currentGauge) //Random.value < 현재 확률 
        //{

        //    //돌발행동 발생
        //    //minOutburstMult와 maxOutburstMult 사이의 임의의 배율을 선택된다.
        //    float randomMultiplier = Random.Range(minOutburstMult, maxOutburstMult);

        //    Debug.Log("[돌발행동] 발생 (확률 초기화), 힘 배율 : " + (randomMultiplier).ToString("F1") + "배");
        //    currentGauge = 0.0f; // 발동 후 다시 초기화

        //    return randomMultiplier; // 돌발 행동이 터졌다고 알려줌
        //}
        //else
        //{

        //    // 돌발행동이 발생하지 않는다면 확률을 높여준다.
        //    float added = Random.Range(addMin, addMax);
        //    //Debug.Log("증가된 확률" + (added * 100f).ToString("F1") + "%");
        //    currentGauge += added; // 돌발행동할 확률을 증가시킨다.

        //    // 확률이 최대 100%(1.0)이 넘지 않도록 안전장치를 걸어준다.
        //    currentGauge = Mathf.Clamp01(currentGauge);

        //    Debug.Log("다음 돌발행동 확률 : " + (currentGauge * 100f).ToString("F1") + "%");

        //    return 1.0f; // 돌발행동이 생기지 않음, 1.0으로 하는 이유는 힘이 추가로 받지 않는 일반 점프임으로 1.0을 곱해 힘 배율이 변화없게끔 설정
        //}
        #endregion

        // 변경 : 돌발행동하는 조건이 확률이 아닌 100%가 되었을 때 실행되게 끔 설정하고 100% 되는 순간 작동하게끔 변경

        // 이번 점프에서 추가될 게이지를 먼저 계산해서 더한다.
        float added = Random.Range(addMin, addMax);

        currentGauge += added;

        if (currentGauge >= 1.0f) // 현재 확률이 1.0 = 100%가 되가 넘을 때 발동
        {
            //IsOutburst = true; // 일단 사용안하고 있음

            // 93%에서 10%가 더해져 103%가 된 그 순간! 돌발행동이 터집니다.
            float randomMultiplier = Random.Range(minOutburstMult, maxOutburstMult);

            // [핵심] "돌발행동 터졌다!"라고 신호를 방송합니다.
            OnOutburstTriggered?.Invoke(randomMultiplier); // 이벤트 시스템: 추가 공부 필요!!!!!

            // [사운드 추가] 돌발행동 효과음 재생!
            if (sfxAudioSource != null && outburstSound != null)
            {
                sfxAudioSource.PlayOneShot(outburstSound);
            }

            Debug.Log("[돌발행동] 발동! (게이지 초기화), 힘 배율 : " + (randomMultiplier).ToString("F1") + "배");

            // 발동되었으니 게이지를 다시 0으로 초기화합니다.
            currentGauge = 0.0f;
            return randomMultiplier;
        }
        else
        {
            
            //IsOutburst = false; // 일단 사용안하고 있음
            return 1.0f; // 돌발행동이 생기지 않음, 1.0으로 하는 이유는 힘이 추가로 받지 않는 일반 점프임으로 1.0을 곱해 힘 배율이 변화없게끔 설정
        }

        

    }
}
