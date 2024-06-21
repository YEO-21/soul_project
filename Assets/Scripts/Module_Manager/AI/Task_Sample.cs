using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Sample : RunnableBehavior
{
    private string _Print;


    public Task_Sample(string print)
    {
        _Print = print;
    }


    public override IEnumerator OnBehaivorStarted()
    {
        Debug.Log(_Print);

        // 실행 성공
        isSucceeded = true;

        yield break;
    }
}


