using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// 이 형태의 객체를 나타냅니다.
    /// </summary>
    private static GameManagerBase _ThisInstance;

    public static GameManagerBase Get() => Get<GameManagerBase>();

    public static T Get<T>() where T : GameManagerBase
    {
        if(!_ThisInstance)
        {
            // 월드에서 GameManagerBase 형태의 객체(컴포넌트)를 찾습니다.
            _ThisInstance = FindObjectOfType<GameManagerBase>();
        }
        // GameManager 객체를 반환합니다.
        return _ThisInstance as T;
    }


}
