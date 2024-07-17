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

/*
Root (Selector)


�÷��̾� ���� �ȵ� (Sequencer)
- ���� ������ ������ ��ġ�� �̽��ϴ�. (Task)
- �ش� ��ġ�� �̵� (Task)
- ��� (Task)

�÷��̾� ������ (Selector)
- ���� ������ ���� ���ο� �÷��̾ �ִٸ� (Selector)
  - �÷��̾ ��� ������ ���
    - ���� ���� (Task)
  - �÷��̾ ��� ���°� �ƴ� ���
    - ���� ���� (Task)
- ���� ������ ���� ���ο� �÷��̾ ���ٸ� (Sequencer)
  - �÷��̾� ��ġ�� ����ϴ�. (Task)
  - �÷��̾� ��ġ�� �̵� (Task)
 */