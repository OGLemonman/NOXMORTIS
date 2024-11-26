using UnityEngine;

public class SlowAttackingState : StateBase
{
    private SlowEnemyController controller;
    private float lastAttack;
    private bool attacking;

    public SlowAttackingState(SlowEnemyController _controller) {
        controller = _controller;
    }

    private void Attack() {
        attacking = true;
        lastAttack = Time.realtimeSinceStartup;
        // TODO: Implement attack logic
        attacking = false;
    }

    public override void OnUpdate() {
        if (attacking) return;

        if (!SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.attackSightDistance, controller.attackSightCone)) {
            if (SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.followSightDistance, controller.followSightCone)) {
                controller.ChangeState("Following");
            } else {
                controller.ChangeState("Patrolling");
            }
            return;
        }

        bool canAttack = Time.realtimeSinceStartup - lastAttack > controller.attackCooldown;
        if (!canAttack) {
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, controller.target.position, controller.walkSpeed * Time.deltaTime);
            controller.transform.LookAt(new Vector3(controller.target.position.x, controller.transform.position.y, controller.target.position.z));
        } else {
            Attack();
        }
    }
}