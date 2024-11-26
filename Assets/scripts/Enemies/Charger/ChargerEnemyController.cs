using UnityEngine;
using System.Collections;

public class ChargerEnemyController : MonoBehaviour
{
    public Animator animator;
    public LayerMask agentMask;
    public float walkSpeed = 5f;
    public Transform target;
    public ChargerEnemyGizmosSettings gizmosSettings;
    public float sightDistance = 10f;
    public float sightCone = 90f;
    public float hearDistance = 5f;
    public float hearCone = 360f;
    public float chargedDist = 20f;
    public float chargeSpeed = 9f;
    public float attackCooldown = 3f;
    public float damage = 70f;
    private float lastAttack;
    private Rigidbody rb;
    private StateMachine stateMachine;

    void Start() {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        stateMachine.AddState("Patrolling", new ChargerPatrollingState(this));
        stateMachine.AddState("Attacking", new ChargerAttackingState(this));
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

        // Hearing
        if (gizmosSettings.hearCone) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, hearDistance);

            Vector3 vectorA = Quaternion.AngleAxis(hearCone/2, Vector3.up) * transform.forward;
            Vector3 vectorB = Quaternion.AngleAxis(-hearCone/2, Vector3.up) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position  + vectorA * hearDistance);
            Gizmos.DrawLine(transform.position, transform.position  + vectorB * hearDistance);
        }
    }

    public void TryAttack() {
        if (Time.realtimeSinceStartup - lastAttack > attackCooldown) {
            lastAttack = Time.realtimeSinceStartup;
            StartCoroutine(Attack());
        } else {
            ChangeState("Patrolling");
        }
    }

    IEnumerator Attack() {
        animator.SetInteger("State", 0);
        Vector3 destination = transform.position + (target.position - transform.position).normalized * chargedDist;
        transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));

        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("State", 1);

        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, destination, chargeSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));

            if ((transform.position - destination).magnitude < 2) break;
            Debug.Log("o");
            yield return null;
        }

        animator.SetInteger("State", 0);

        yield return new WaitForSeconds(1);
        ChangeState("Patrolling");
        yield return null;
    }

    public void ChangeState(string stateName) {
        stateMachine.ChangeState(stateName);
    }

    [System.Serializable]
    public struct ChargerEnemyGizmosSettings {
        public bool patrolCone;
        public bool hearCone;
    }
}