using UnityEngine;

public class SlowFollowingState : StateBase
{
    private SlowEnemyController controller;

    public SlowFollowingState(SlowEnemyController _controller) {
        controller = _controller;
    }

    public override void OnUpdate() {
        if (!SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.followSightDistance, controller.followSightCone)) {
            controller.ChangeState("Patrolling");
            return;
        } else if (SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.attackSightDistance, controller.attackSightCone)) {
            controller.ChangeState("Attacking");
            return;
        }

        controller.transform.position = Vector3.MoveTowards(controller.transform.position, controller.target.position, controller.walkSpeed * Time.deltaTime);
        controller.transform.LookAt(new Vector3(controller.target.position.x, controller.transform.position.y, controller.target.position.z));
    }
}