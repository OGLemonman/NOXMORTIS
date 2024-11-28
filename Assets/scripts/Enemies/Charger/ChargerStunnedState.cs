using UnityEngine;

public class ChargerStunnedState : StateBase
{
    private ChargerEnemyController controller;
    private float startTime;

    public ChargerStunnedState(ChargerEnemyController _controller) {
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