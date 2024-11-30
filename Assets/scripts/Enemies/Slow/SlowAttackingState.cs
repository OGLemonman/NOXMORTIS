using UnityEngine;

public class SlowAttackingState : StateBase
{
    private SlowEnemyController controller;

    public SlowAttackingState(SlowEnemyController _controller) {
        controller = _controller;
    }

    public override void OnEnter() {
        controller.TryAttack();
    }
}