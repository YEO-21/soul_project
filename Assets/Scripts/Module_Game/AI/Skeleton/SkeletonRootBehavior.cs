using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonRootBehavior : BehaviorSelector
{
    public SkeletonRootBehavior()
    {
        // 플레이어를 감지하지 않았을 경우
        AddBehavior<SkeletonNormalSequencer>();

        // 플레이어를 감지한 경우
        AddBehavior<SkeletonAggressiveSelector>();
    }

}

/*
Root (Selector)


플레이어 감지 안됨 (Sequencer)
- 범위 내에서 랜덤한 위치를 뽑습니다. (Task)
- 해당 위치로 이동 (Task)
- 대기 (Task)

플레이어 감지됨 (Selector)
- 공격 가능한 범위 내부에 플레이어가 있다면 (Selector)
  - 플레이어가 방어 상태인 경우
    - 가드 실행 (Task)
  - 플레이어가 방어 상태가 아닌 경우
    - 공격 실행 (Task)
- 공격 가능한 범위 내부에 플레이어가 없다면 (Sequencer)
  - 플레이어 위치를 얻습니다. (Task)
  - 플레이어 위치로 이동 (Task)
 */