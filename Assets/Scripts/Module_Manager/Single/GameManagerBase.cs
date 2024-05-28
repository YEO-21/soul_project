using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// �� ������ ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private static GameManagerBase _ThisInstance;

    public static GameManagerBase Get() => Get<GameManagerBase>();

    public static T Get<T>() where T : GameManagerBase
    {
        if(!_ThisInstance)
        {
            // ���忡�� GameManagerBase ������ ��ü(������Ʈ)�� ã���ϴ�.
            _ThisInstance = FindObjectOfType<GameManagerBase>();
        }
        // GameManager ��ü�� ��ȯ�մϴ�.
        return _ThisInstance as T;
    }


}
