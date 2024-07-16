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

    /// <summary>
    ///  UI ���� Ÿ�̸� ��ƾ
    /// </summary>
    private Coroutine _HideUITimerRoutine;

    /// <summary>
    /// Ÿ�̸� ���� �� ����ų �ð��� ��Ÿ���ϴ�.
    /// </summary>
    private WaitForSecondsRealtime _HideUISeconds;



    public void InitializeUI(GameScenePlayerController playerController)
    {
        PlayerCharacter playerCharacter = (playerController.controlledCharacter as PlayerCharacter);

        // ��ü ���� �̺�Ʈ�� �Լ� ���ε�
        playerCharacter.attack.onNewDamageableDetected += CALLBACK_OnNewDamageableDetected;

        // Ÿ�̸� ���� �� ���� ��ü�� �̿��Ͽ� ����Ű���� ��� ��ü�� �̸� �����صӴϴ�.
        _HideUISeconds = new WaitForSecondsRealtime(3.0f);

        // UI �� ����ϴ�.
        SetUIVisibility(false);
    }




    /// <summary>
    /// �ش� UI �� ȭ�鿡 ǥ���մϴ�.
    /// </summary>
    public void SetUIVisibility(bool visible)
    {
        if (visible)
        {
            // UI �� ǥ���մϴ�.
            gameObject.SetActive(true);

            // ���� �������� ��ƾ�� �����Ѵٸ� ���� ���
            if (_HideUITimerRoutine != null)
            {
                StopCoroutine(_HideUITimerRoutine);
                _HideUISeconds.Reset();
            }

            // Ÿ�̸Ӹ� ������մϴ�.
            _HideUITimerRoutine = StartCoroutine(SetHideUITimer());
        }
        // UI �� ����ϴ�.
        else gameObject.SetActive(false);


    }

    private IEnumerator SetHideUITimer()
    {
        // ������ �ð���ŭ ���
        yield return _HideUISeconds;

        // UI�� ����ϴ�.
        SetUIVisibility(false);
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

        // UI �� ǥ���մϴ�.
        SetUIVisibility(true);


    }




}
