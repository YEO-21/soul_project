using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerStateUI : MonoBehaviour
{
    [Header("# ü�� �̹���")]
    public Image m_HpImage;

    public void InitializeUI(GameScenePlayerController playerController)
    {
        GameScenePlayerState playerState = playerController.playerState as GameScenePlayerState;
        playerState.onHpChanged += CALLBACK_OnHpChanged;
    }

    public void CALLBACK_OnHpChanged(float maxHp, float hp)
    {
        m_HpImage.fillAmount = hp / maxHp;
    }



}