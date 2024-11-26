using UnityEngine;

public class NormalAttackingState : StateBase
{
    private NormalEnemyController controller;

    public NormalAttackingState(NormalEnemyController _controller) {
        controller = _controller;
    }

    public override void OnEnter() {
        controller.TryAttack();
    }
}