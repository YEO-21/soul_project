using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ĳ���͸� �����ϱ� ���� ��Ʈ�ѷ� ������Ʈ�Դϴ�.
/// �⺻������ ����� �Է��� �޾�, ĳ���Ϳ��� �����մϴ�.
/// </summary>
public class PlayerControllerBase : MonoBehaviour
{
    /// <summary>
    /// �������� �÷��̾� ĳ���� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
   public PlayerCharacterBase controlledCharacter { get; private set; }

    /// <summary>
    /// ĳ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="controlCharacter"></param>
   public virtual void StartControlCharacter(PlayerCharacterBase controlCharacter)
   {
        // ���� �������� ĳ���Ϳ� ���� ĳ���͸� ���� �����Ϸ��� �ϴ� ��� �Լ� ȣ�� ����
        if (controlledCharacter == controlCharacter) return;
        
        // �̹� Ư���� ĳ���͸� �������� ���
        if (controlledCharacter)
        {
            // ĳ���� ������ �����ϴ�.
            FinishControlCharacter();
        }

        // ���Ӱ� ������ų ĳ���͸� �����մϴ�.
        controlledCharacter = controlCharacter;

        // ĳ���� ������ ���۽�ŵ�ϴ�.
        controlCharacter.OnControlStarted(this);
   }

   /// <summary>
    /// ������ �����ϴ�.
    /// </summary>
   public virtual void FinishControlCharacter()
   {
        // �������� ĳ���Ͱ� �������� ���� ��� �Լ� ȣ�� ����
        if (controlledCharacter == null) return;

        // ĳ���� ������ �����ϴ�.
        controlledCharacter.OnControlFinished();

        // ���� ĳ���� ����
        controlledCharacter = null;
   }


}


