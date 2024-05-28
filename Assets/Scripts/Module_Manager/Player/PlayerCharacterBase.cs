using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾� ĳ���͸� ��Ÿ���� ���� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerCharacterBase : MonoBehaviour
{
    /// <summary>
    /// �� ĳ���͸� �������� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    public PlayerControllerBase playerController { get; private set; }

    /// <summary>
    /// �� ĳ������ ������ ���۵Ǿ��� ��� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="playerController"></param>
    public virtual void OnControlStarted(PlayerControllerBase playerController)
        => this.playerController = playerController;

    /// <summary>
    /// �� ĳ������ ������ ������ ��� ȣ��˴ϴ�.
    /// </summary>
    public virtual void OnControlFinished() => playerController = null;



}
