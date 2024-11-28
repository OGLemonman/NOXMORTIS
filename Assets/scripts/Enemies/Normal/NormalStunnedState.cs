using UnityEngine;

public class NormalStunnedState : StateBase
{
    private NormalEnemyController controller;
    private float startTime;

    public NormalStunnedState(NormalEnemyController _controller) {
        controller = _controller;
    }

    public override void OnEnter() {
        startTime = Time.realtimeSinceStartup;
        controller.animator.SetInteger("State", 0);
    }

    public override void OnUpdate() {
        if (Time.realtimeSinceStartup - startTime > 3) {
            controller.ChangeState("Patrolling");
        }
    }
}