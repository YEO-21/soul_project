using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 캐릭터를 조종하기 위한 컨트롤러 컴포넌트입니다.
/// 기본적으로 사용자 입력을 받아, 캐릭터에게 전달합니다.
/// </summary>
public class PlayerControllerBase : MonoBehaviour
{
    /// <summary>
    /// 조종중인 플레이어 캐릭터 객체를 나타냅니다.
    /// </summary>
   public PlayerCharacterBase controlledCharacter { get; private set; }

    /// <summary>
    /// 캐릭터 조종을 시작합니다.
    /// </summary>
    /// <param name="controlCharacter"></param>
   public virtual void StartControlCharacter(PlayerCharacterBase controlCharacter)
   {
        // 현재 조종중인 캐릭터와 같은 캐릭터를 조종 시작하려고 하는 경우 함수 호출 종료
        if (controlledCharacter == controlCharacter) return;
        
        // 이미 특정한 캐릭터를 조종중인 경우
        if (controlledCharacter)
        {
            // 캐릭터 조종을 끝냅니다.
            FinishControlCharacter();
        }

        // 새롭게 조종시킬 캐릭터를 지정합니다.
        controlledCharacter = controlCharacter;

        // 캐릭터 조종을 시작시킵니다.
        controlCharacter.OnControlStarted(this);
   }

   /// <summary>
    /// 조종을 끝냅니다.
    /// </summary>
   public virtual void FinishControlCharacter()
   {
        // 조종중인 캐릭터가 존재하지 않을 경우 함수 호출 종료
        if (controlledCharacter == null) return;

        // 캐릭터 조종을 끝냅니다.
        controlledCharacter.OnControlFinished();

        // 조종 캐릭터 비우기
        controlledCharacter = null;
   }


}


