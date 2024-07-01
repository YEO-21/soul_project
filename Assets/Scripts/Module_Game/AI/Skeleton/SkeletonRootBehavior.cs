using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class SkeletonRootBehavior : BehaviorSelector
{
    public SkeletonRootBehavior()
    {
        // �÷��̾ �������� �ʾ��� ���
        AddBehavior<SkeletonNormalSequencer>();

       
       // �÷��̾ ������ ���
       AddBehavior<SkeletonAggressiveSelector>();

    }
   

}
