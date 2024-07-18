using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerStateUI : MonoBehaviour
{
    [Header("# 체력 이미지")]
    public Image m_HpImage;

    public void InitializeUI(GameScenePlayerController playerController)
    {
       GameScenePlayerState playerState =  playerController.playerState as GameScenePlayerState;


        playerState.onHpChanged += CALLBACK_OnHpChanged;
    }

    public void CALLBACK_OnHpChanged(float maxHp, float hp)
    {
        float percentHp = hp / maxHp;
        
        m_HpImage.fillAmount = percentHp;


    }


}
