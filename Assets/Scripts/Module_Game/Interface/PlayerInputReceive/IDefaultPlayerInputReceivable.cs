using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⺻���� �÷��̾� �Է��� ���� �� �ִ� ��ü�� ��Ÿ���� ����
/// ���Ǵ� �������̽��Դϴ�.
/// </summary>
public interface IDefaultPlayerInputReceivable
{
    // �̵�
    void OnMovementInput(Vector2 inputAxis);

    // ���� �Է�
    void OnJumpInput();

    // �ȱ� �޸���
    void OnSprintInput(bool isPressed);

    // �⺻ ����
    void OnNormalAttackInput();

}
