using UnityEngine;

public class SlowEnemyController : MonoBehaviour
{
    public LayerMask agentMask;
    public float walkSpeed = 5f;
    public Transform target;
    public SlowEnemyGizmosSettings gizmosSettings;
    public float sightDistance = 10f;
    public float sightCone = 90f;
    public float followSpeed = 6f;
    public float followSightDistance = 20f;
    public float followSightCone = 120f;
    public float attackSightDistance = 5f;
    public float attackSightCone = 60f;
    public float attackCooldown = 3f;
    private Rigidbody rb;
    private StateMachine stateMachine;

    void Start() {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        stateMachine.AddState("Patrolling", new SlowPatrollingState(this));
        stateMachine.AddState("Following", new SlowFollowingState(this));
        stateMachine.AddState("Attacking", new SlowAttackingState(this));
        stateMachine.ChangeState("Patrolling");
    }

    void Update() {
        stateMachine.OnUpdate();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);

        // Patrolling
        if (gizmosSettings.patrolCone) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, sightDistance);

            Vector3 vectorA = Quaternion.AngleAxis(sightCone/2, Vector3.up) * transform.forward;
            Vector3 vectorB = Quaternion.AngleAxis(-sightCone/2, Vector3.up) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position  + vectorA * sightDistance);
            Gizmos.DrawLine(transform.position, transform.position  + vectorB * sightDistance);
        }

        // Following
        if (gizmosSettings.followCone) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, followSightDistance);

            Vector3 vectorC = Quaternion.AngleAxis(followSightCone/2, Vector3.up) * transform.forward;
            Vector3 vectorD = Quaternion.AngleAxis(-followSightCone/2, Vector3.up) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position  + vectorC * followSightDistance);
            Gizmos.DrawLine(transform.position, transform.position  + vectorD * followSightDistance);
        }

        // Attacking
        if (gizmosSettings.attackCone) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackSightDistance);

            Vector3 vectorE = Quaternion.AngleAxis(attackSightCone/2, Vector3.up) * transform.forward;
            Vector3 vectorF = Quaternion.AngleAxis(-attackSightCone/2, Vector3.up) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position  + vectorE * attackSightDistance);
            Gizmos.DrawLine(transform.position, transform.position  + vectorF * attackSightDistance);
        }
    }

    public void ChangeState(string stateName) {
        stateMachine.ChangeState(stateName);
    }

    [System.Serializable]
    public struct SlowEnemyGizmosSettings {
        public bool patrolCone;
        public bool followCone;
        public bool attackCone;
    }
}