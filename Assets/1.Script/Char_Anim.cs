using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Anim : MonoBehaviour
{
    private Animator charAnim;
    private CharMove charMoveScript;


    private void Awake()
    {
        // 동일한 게임 오브젝트에 붙어있는 Animator와 CharMove 컴포넌트를 가져온다.
        charAnim = GetComponent<Animator>();
        charMoveScript = GetComponent<CharMove>();
    }

    private void Update()
    {
        CharAnimation();
    }

    private void CharAnimation()
    {
        if (charMoveScript == null) return;

        // 2.CharMove의 변수값을 그대로 애니메이터에 전달!
        charAnim.SetBool("isWalking", charMoveScript.isWalking);

        // 점프 애니메이션 연결할 때 활성화
        charAnim.SetBool("isJumping", charMoveScript.isJumping);

        // 점프 기모으고 있을 때
        charAnim.SetBool("isCharging", charMoveScript.isCharging);
    }

}
