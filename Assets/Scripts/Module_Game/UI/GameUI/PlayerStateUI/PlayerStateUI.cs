using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerStateUI : MonoBehaviour
{
    [Header("# ü�� �̹���")]
    public Image m_HpImage;

    [Header("# ���¹̳� �̹���")]
    public Image m_StaminaImage;

    public void InitializeUI(GameScenePlayerController playerController)
    {
        GameScenePlayerState playerState = playerController.playerState as GameScenePlayerState;
        playerState.onHpChanged += CALLBACK_OnHpChanged;
        playerState.onStaminaChanged += CALLBACK_OnStaminaChanged;
    }

    public void CALLBACK_OnHpChanged(float maxHp, float hp)
    {
        m_HpImage.fillAmount = hp / maxHp;
    }

    public void CALLBACK_OnStaminaChanged(float maxStamina, float stamina)
    {
        m_StaminaImage.fillAmount = stamina / maxStamina;
    }



}
