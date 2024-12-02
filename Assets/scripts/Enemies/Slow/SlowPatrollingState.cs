using UnityEngine;

public class SlowPatrollingState : StateBase
{
    private SlowEnemyController controller;
    private Vector3 origin;
    private bool walking;
    private float lastWalkTime;
    private float randomWaitTime;
    private Vector3 destination;
    private float lastGrowl = -1000f;
    private float growlCooldown;
    
    

    public SlowPatrollingState(SlowEnemyController _controller) {
        controller = _controller;
        origin = _controller.transform.position;
    }

    private bool GetNewDestination(out Vector3 _destination) {
        Vector3 trialDestination = new Vector3(
            origin.x + Random.Range(-10f, 10f),
            origin.y + 10,
            origin.z + Random.Range(-10f, 10f)
        );

        Ray ray = new Ray(trialDestination, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, ~controller.agentMask)) {
            _destination = hit.point;
            return true;
        }

        _destination = Vector3.zero;
        return false;
    }

    public override void OnUpdate() {
        if (!controller.inImmune) {
            if (SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.sightDistance, controller.sightCone) ||
                SightCheck.IsInSight(controller.transform, controller.target.transform.position, controller.hearDistance, controller.hearCone)) {
                controller.ChangeState("Following");
                return;
            }
        }

        if (!walking) {
            float timeElapsed = Time.realtimeSinceStartup - lastWalkTime;
            if (timeElapsed < randomWaitTime) {
                controller.animator.SetInteger("State", 0);
                return;
            }

            if (GetNewDestination(out Vector3 _destination)) {
                destination = _destination;
                walking = true;
            }
        } else {
            controller.animator.SetInteger("State", 1);
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, destination, controller.walkSpeed * Time.deltaTime);
            controller.transform.LookAt(new Vector3(destination.x, controller.transform.position.y, destination.z));

            float dist = (destination - controller.transform.position).sqrMagnitude;
            if (dist < 2) {
                lastWalkTime = Time.realtimeSinceStartup;
                randomWaitTime = Random.Range(2f, 8f);
                walking = false;
            }

            if (Time.realtimeSinceStartup - lastGrowl > growlCooldown) {
            lastGrowl = Time.realtimeSinceStartup;
            growlCooldown = Random.Range(4f, 12f);
            controller.slowMiscSource.PlayOneShot(controller.slowGrowlAudio);
        }
        }
    }
}