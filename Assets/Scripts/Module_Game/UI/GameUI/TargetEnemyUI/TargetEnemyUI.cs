using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TargetEnemyUI : MonoBehaviour
{
    [Header("# 적 이름 텍스트")]
    public TMP_Text m_NameText;


    [Header("# 적 체력 이미지")]
    public Image m_HpImage;


    public void InitializeUI(GameScenePlayerController playerController)
    {
        PlayerCharacter playerCharacter = (playerController.controlledCharacter as PlayerCharacter);

        // 객체 감지 이벤트에 함수 바인딩
        playerCharacter.attack.onNewDamageableDetected += CALLBACK_OnNewDamageableDetected;
    }




    /// <summary>
    /// 해당 UI 를 화면에 표시합니다.
    /// </summary>
    public void SetUIVisibility(bool visible)
    {

    }

    /// <summary>
    /// 적의 정보를 설정합니다.
    /// </summary>
    private void CALLBACK_OnNewDamageableDetected(IDamageable newDamageable)
    {


        // 적 이름 표시
        m_NameText.text = newDamageable.objectName;
        

        // 적 헌재 체력 표시
        m_HpImage.fillAmount = newDamageable.currentHp / newDamageable.maxHp;


    }




}
