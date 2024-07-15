using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TargetEnemyUI : MonoBehaviour
{
    [Header("# �� �̸� �ؽ�Ʈ")]
    public TMP_Text m_NameText;


    [Header("# �� ü�� �̹���")]
    public Image m_HpImage;


    public void InitializeUI(GameScenePlayerController playerController)
    {
        PlayerCharacter playerCharacter = (playerController.controlledCharacter as PlayerCharacter);

        // ��ü ���� �̺�Ʈ�� �Լ� ���ε�
        playerCharacter.attack.onNewDamageableDetected += CALLBACK_OnNewDamageableDetected;
    }




    /// <summary>
    /// �ش� UI �� ȭ�鿡 ǥ���մϴ�.
    /// </summary>
    public void SetUIVisibility(bool visible)
    {

    }

    /// <summary>
    /// ���� ������ �����մϴ�.
    /// </summary>
    private void CALLBACK_OnNewDamageableDetected(IDamageable newDamageable)
    {


        // �� �̸� ǥ��
        m_NameText.text = newDamageable.objectName;
        

        // �� ���� ü�� ǥ��
        m_HpImage.fillAmount = newDamageable.currentHp / newDamageable.maxHp;


    }




}
