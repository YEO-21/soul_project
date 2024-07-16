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

    /// <summary>
    ///  UI 숨김 타이머 루틴
    /// </summary>
    private Coroutine _HideUITimerRoutine;

    /// <summary>
    /// 타이머 동작 시 대기시킬 시간을 나타냅니다.
    /// </summary>
    private WaitForSecondsRealtime _HideUISeconds;



    public void InitializeUI(GameScenePlayerController playerController)
    {
        PlayerCharacter playerCharacter = (playerController.controlledCharacter as PlayerCharacter);

        // 객체 감지 이벤트에 함수 바인딩
        playerCharacter.attack.onNewDamageableDetected += CALLBACK_OnNewDamageableDetected;

        // 타이머 동작 시 같은 객체를 이용하여 대기시키도록 대기 객체를 미리 생성해둡니다.
        _HideUISeconds = new WaitForSecondsRealtime(3.0f);

        // UI 를 숨깁니다.
        SetUIVisibility(false);
    }




    /// <summary>
    /// 해당 UI 를 화면에 표시합니다.
    /// </summary>
    public void SetUIVisibility(bool visible)
    {
        if (visible)
        {
            // UI 를 표시합니다.
            gameObject.SetActive(true);

            // 전에 실행중인 루틴이 존재한다면 실행 취소
            if (_HideUITimerRoutine != null)
            {
                StopCoroutine(_HideUITimerRoutine);
                _HideUISeconds.Reset();
            }

            // 타이머를 재실행합니다.
            _HideUITimerRoutine = StartCoroutine(SetHideUITimer());
        }
        // UI 를 숨깁니다.
        else gameObject.SetActive(false);


    }

    private IEnumerator SetHideUITimer()
    {
        // 지정한 시간만큼 대기
        yield return _HideUISeconds;

        // UI를 숨깁니다.
        SetUIVisibility(false);
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

        // UI 를 표시합니다.
        SetUIVisibility(true);


    }




}
