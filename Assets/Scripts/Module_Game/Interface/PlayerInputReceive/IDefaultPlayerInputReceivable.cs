using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⺻���� �÷��̾� �Է��� ���� �� �ִ� ��ü�� ��Ÿ���� ����
/// ���Ǵ� �������̽��Դϴ�.
/// </summary>
public interface IDefaultPlayerInputReceivable
{
    /// <summary>
    /// �̵�
    /// </summary>
    /// <param name="inputAxis"></param>
    void OnMovementInput(Vector2 inputAxis);

    /// <summary>
    /// ���� �Է�
    /// </summary>
    void OnJumpInput();

    /// <summary>
    /// �ȱ� �޸���
    /// </summary>
    /// <param name="isPressed"></param>
    void OnSprintInput(bool isPressed);

    /// <summary>
    /// �⺻ ����
    /// </summary>
    void OnNormalAttackInput();

    /// <summary>
    /// ù��° ������ ���
    /// </summary>
    void OnUseItem1();

    /// <summary>
    /// ��� �Է�
    /// </summary>
    /// <param name="isPressed"></param>
    void OnGuardInput(bool isPressed);

    /// <summary>
    /// ��ȣ�ۿ� �Է�
    /// </summary>
    void OnInteractInput();

    /// <summary>
    /// UI �ݱ� �Է�
    /// </summary>
    void OnCloseUIInput();

}
