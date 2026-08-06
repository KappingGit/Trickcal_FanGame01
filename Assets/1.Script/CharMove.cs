using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    // 기본적인 캐릭터 움직임 관련 코드
    // 점프킹 게임 특성상 캐릭터의 움직임은 관성 기능 없이 바로 멈춰야 하는 특성을 가지고 있다
    // 그렇기에 rigidbody.velocity 모드를 채택한다.
    // [SerializeField] 주의사항: 인스펙터에 있는 데이터 값을 우선 순위로 한다.

    private Rigidbody2D charRb;
    private SpriteRenderer spriteRenderer;

    //---------------------------------------------------------
    // 애니메이션에 활용할 캐릭터 상태 확인용
    [HideInInspector] public bool isWalking; // 걷는 중
    [HideInInspector] public bool isJumping; // 점프 중
    [HideInInspector] public bool isCharging; // 기 모으는 중
    //---------------------------------------------------------

    [Header("캐릭터 이동속도")]
    [SerializeField]private float speed = 5f; // 기본 속도 값

    private float char_X; // float인 이유 Input.GetAxisRaw("Horizontal")의 경우 키보드 좌우보단 조이스틱 좌우 개념으로 접근한다.
    // 패드의 아날로그 조이스틱 입력까지 모두 소화하고 유니티의 다른 물리 연산들과 쉽게 어울리기 위해 float 타입을 사용

    [Header("지면 감지 레이어")]
    [SerializeField]  private LayerMask groundLayer; // 감지할 땅의 레이어, 인스펙터 창에서 레이어 마스크를 Ground으로 설정
    // 아래 주석은 위 방식에서 [SerializeField]를 사용하지 않고 오로지 c코드로 사용하는 방법
    // [SerializeField]를 지우고 Awake에서 groundLayer = LayerMask.GetMask("Ground");을 사용
    // 또 다른 방법이 있지만 2진수 계산방식(비트 시트)를 활용하는거라 이 부분은 제외 단, 이 방법은 가벼운 방식으로 활용됨(최적화, 하지만 가독성 떨어짐) 

    //레이 캐스트를 위한 변수
    private float rayDistance = 1.0f; // 감지할 걸이 길이
    
    private bool isGrounded; // 감지 되었는지 여부 확인

    [Header("점프 설정")] // 헤더 방식으로 인스펙터 창의 가독성을 높임
    private float jumpForce = 0f; // 현재 모인 점프 힘(점프 키를 누르는 시간 경과에 따른 힘)
    private float maxJumpForce = 14f; // 최대 힘 제한(무한정으로 힘을 누적하는 것을 방지)
    private float chargeSpeed = 18f; // 점프 힘이 모이는 속도(게이지가 얼마나 차오르는지 결정하는 변수)

    [Header("박스 캐스트 설정")]
    // 박스 캐스트를 위한 변수
    [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.1f); // 가로 세로 크기

    [Tooltip("지면 탐지용 Box콜라이더 Y값을 참고에서 입력")]
    [SerializeField] private float boxCastDistance = 0.5f; // 박스 캐스트가 쏘아질 거리

    [Header("TumblerOfShasha 스크립트")]
    //TumblerOfShasha 스크립트 변수
    [SerializeField] private TumblerOfShasha tumblerScript;

    private void Awake()
    {
        charRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 같은 오브젝트(또는 캐릭터)에 붙어있는 TumblerOfShasha 스크립트 컴포넌트를 가져온다
        // tumblerScript = GetComponent<TumblerOfShasha>();

        if (tumblerScript == null)
        {
            Debug.LogWarning("경고 : TumblerOfShasha의 스크립트가 없습니다.");
        }

    }

    private void Update()
    {

        CheckGrounded(); // 지면 감지용 박스 레이캐스트

        Move(); // 기본 조작키

        //RaycastHit(); //레이캐스트 히트(사용안함)
    }

    
    private void Move()
    {
        char_X = Input.GetAxisRaw("Horizontal"); // 유니티에서 제공되는 기본 움직임

        if (isGrounded) // 공중에서 좌우로 움직이는 것을 방지
        {

            if (Input.GetKey(KeyCode.Space)) //스페이스바를 꾹 누르고 있다면...(기를 모으는 중이라면)
            {
                isWalking = false;
                isCharging = true;

                charRb.velocity = new Vector2(0f, charRb.velocity.y); // 발을 땅에 고정시키기

                jumpForce += chargeSpeed * Time.deltaTime; // 매 프레임에 속도를 추가한다.
                jumpForce = Mathf.Min(jumpForce, maxJumpForce); // 점프 게이지의 최대치 제한하기 해당 키워드는 둘 중에 가장 작은 값을 출력
                Debug.Log("점프 게이지 모으는 중 " + jumpForce);

            }
            else if(Input.GetKeyUp(KeyCode.Space)) // 스페이스바를 때는 순간 점프 실행하게끔
            {
                Jump();

                #region (사용안함) 단순 normalized 사용기법 [문제점: 좌우 점프와 제자리 점프의 힘이 다르게 작용]

                /*
                // 발사 방향 계산: 앞서 설정한 좌우 입력(char_X)과 위쪽(1.2f 비율)을 조합해 대각선 벡터 생성
                // .normalized를 붙여주면 방향 순수 힘만 남겨 정교한 계산이 가능
                Vector2 jumpDirection = new Vector2(char_X, 1.2f).normalized;

                // 키워드: ForceMode2D.Impulse (순간적인 충격량 부여)
                // 리지드바디에게 (방향 * 모은 힘) 만큼의 속도를 순간적으로 팍 밀어 넣어 날린다
                charRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
                */

                #endregion

                #region (사용안함)
                // 기본 발사 방향 계산 (기존과 동일)
                //Vector2 jumpDirection = new Vector2(char_X, 1.2f).normalized;

                //// 보정된 최종 힘을 담을 변수
                //float finalForce = jumpForce;

                //// 만약 좌우 입력이 있다면 (대각선 점프라면)
                //if (char_X != 0)
                //{
                //    // 줄어든 높이만큼 전체 힘을 뻥튀기해 줍니다.
                //    // (1.2f ~ 1.3f 사이의 값을 조율해가며 제자리 점프와 높이를 맞춥니다.)
                //    finalForce = jumpForce * 1.25f;
                //}

                //// 보정된 힘으로 날리기
                //charRb.AddForce(jumpDirection * finalForce, ForceMode2D.Impulse);
                #endregion

            }
            else // 평범하게 걷고 있다면
            {
               
                if (char_X < 0)  // 좌우 바라보는 방향 처리 (걷고 있을 때만 처리)
                {
                    spriteRenderer.flipX = true;  // 왼쪽 이동 시 이미지 반전
                }
                else if (char_X > 0)
                {
                    spriteRenderer.flipX = false; // 오른쪽 이동 시 원래대로
                }

                isCharging = false;
                isWalking = (char_X != 0);

                charRb.velocity = new Vector2(char_X * speed, charRb.velocity.y);
                jumpForce = 0f; // 걷는 중에는 점프 게이지 확실히 초기화

            }
        }
        else // 공중에 있을 때
        {
            isWalking = false;
            isCharging = false;
            isJumping = true;

            // 공중에서는 AddForce나 속도 제어를 하지 않아 한 번 뛴 궤적을 바꿀 수 없게(점프킹 특성) 만든다
        }

    }

    private void Jump() // 캐릭터 점프
    {
        // 점프의 힘 보정해주는 변수(좌우 점프와 제자리 점프 보정)
        //방향 벡터를 하나로 묶어 정규화(.normalized)하지 않고, 힘을 각각 계산
        float force_Y = jumpForce; // 위로 솟구치는 힘은 언제나 100 % 고정

        // 좌우 힘은 방향(char_X)에 힘을 곱하되, 밸런스를 위해 가로 계수(0.7f)를 곱해 조율
        float force_X = char_X * jumpForce * 0.7f;

        //-----------------------------------------------
        //돌발행동을 한다면...
        // 텀블러 스크립트에서 가져온다.
        if (tumblerScript != null) // 텀블러 스크립트가 제대로 연결되어 있다면 배율을 가져와서 곱한다.
        {
            float multiplier = tumblerScript.Outburst();
            force_Y *= multiplier;
            force_X *= multiplier;

            //    Debug.Log("최종 적용된 점프 힘 배율: " + (multiplier).ToString("F1") + "배");
            //}

            //-----------------------------------------------

            //Vector2 finalJumpForce = new Vector2(force_X, force_Y); // 리지드바디에게 (방향 * 모은 힘) 만큼의 속도를 순간적으로 팍 밀어 넣어 날린다

            // 키워드: ForceMode2D.Impulse (순간적인 충격량 부여)
            //charRb.AddForce(finalJumpForce, ForceMode2D.Impulse);

            //Debug.Log("최종 점프 힘: " + finalJumpForce);
        }
        charRb.velocity = new Vector2(force_X, force_Y);

        isGrounded = false;
        isJumping = true;
        isCharging = false;
        jumpForce = 0f; // 점프 직후 게이지 비우기

    }


    #region (사용 안함) 레이캐스트 히트
    // 키워드 : Physics2D.Raycast(시작위치, 방향, 길이, 필터링할 레이어(감지되는 레이어))
    // 키워드 : Debug.DrawRay(시작위치, 방향 * 길이, 눈으로 확인하기 위한 표시 될 색상)
    private void RaycastHit() //레이캐스트를 활용해서 캐릭터가 지면에 있는지 검사하는 용도
    {
        
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);  // 레이캐스트의 레이저를 시각적으로 확인하는 디버그
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
            Debug.Log("땅에 닫고 있습니다. " + isGrounded);
        }
        else
        {
            isGrounded = false;
            Debug.Log("땅에 안 닫고 있습니다. " + isGrounded);
        }

    }
    #endregion

    // 키워드 : Physics2D.BoxCast(시작 위치 중심, 가로세로 크기, 회전 각도, 박스의 가리키는 방향, 감지 거리, 감지 대상 레이어)
    private void CheckGrounded() //땅을 인식하는 함수 레이캐스트의 단점을 보완하기 위한 박스캐스트히트 함수
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, boxCastDistance, groundLayer);
        
        bool wasGrounded = isGrounded; // 이전 프레임의 상태 저장
        isGrounded = (hit.collider != null);

        // 막 공중에서 땅으로 착지한 순간 (안정성을 위해 힘 초기화)
        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
            jumpForce = 0f;
        }
    }

    
    // 유니티 에디터 Scene 뷰에서 감지 영역을 눈으로 확인하기 위한 디버깅 함수
    private void OnDrawGizmos() // 개발자 전용 도구 함수(이벤트 콜백 함수)로 Update에 안넣어도 된다.
    {
        
        Gizmos.color = Color.red; // Gizmos 색상을 빨간색으로 설정
        
        Vector3 boxPosition = transform.position + (Vector3.down * boxCastDistance); // BoxCast가 실제로 검사하게 될 최하단 위치 계산

        Gizmos.DrawWireCube(boxPosition, new Vector3(boxSize.x, boxSize.y, 0f)); // 해당 위치에 가상의 와이어 프레임(선으로 된 상자)을 그림
    }
    
}
