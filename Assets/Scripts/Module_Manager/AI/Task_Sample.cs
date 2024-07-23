using System.Collections;
using UnityEngine;

public class Task_Sample : RunnableBehavior
{
    private string _Print;

    public Task_Sample(string print)
    {
        _Print = print;
    }

    public override IEnumerator OnBehaviorStarted()
    {
        Debug.Log(_Print);

        // 실행 성공
        isSucceeded = true;

        yield break;
    }
}


