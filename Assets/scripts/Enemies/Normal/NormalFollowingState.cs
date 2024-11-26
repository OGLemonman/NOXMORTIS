using UnityEngine;

public class NormalFollowingState : StateBase
{
    private NormalEnemyController controller;

    public NormalFollowingState(NormalEnemyController _controller) {
        controller = _controller;
    }

    public override void OnEnter() {
        controller.transform.LookAt(new Vector3(controller.target.position.x, controller.transform.position.y, controller.target.position.z));
        controller.animator.SetInteger("State", 0);
    }

    public override void OnUpdate() {
        if (!SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.followSightDistance, controller.followSightCone)) {
            controller.ChangeState("Patrolling");
            return;
        } else if (SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.attackSightDistance, controller.attackSightCone)) {
            controller.ChangeState("Attacking");
            return;
        }

        controller.animator.SetInteger("State", 1);
        controller.transform.position = Vector3.MoveTowards(controller.transform.position, controller.target.position, controller.walkSpeed * Time.deltaTime);
        controller.transform.LookAt(new Vector3(controller.target.position.x, controller.transform.position.y, controller.target.position.z));
    }
}