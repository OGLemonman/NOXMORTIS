using UnityEngine;
using System.Collections;

public class NormalEnemyController : MonoBehaviour
{
    public AudioSource walkSource;
    public AudioSource miscSource;
    public AudioClip growlAudio;
    public AudioClip attackAudio;
    public Animator animator;
    public LayerMask agentMask;
    public float walkSpeed = 5f;
    public Transform target;
    public NormalEnemyGizmosSettings gizmosSettings;
    public float sightDistance = 10f;
    public float sightCone = 90f;
    public float hearDistance = 5f;
    public float hearCone = 360f;
    public float followSpeed = 6f;
    public float followSightDistance = 20f;
    public float followSightCone = 120f;
    public float attackSightDistance = 5f;
    public float attackSightCone = 60f;
    public float attackCooldown = 3f;
    public float damage = 70f;
    public bool inImmune = false;
    private float lastAttack;
    private Rigidbody rb;
    private StateMachine stateMachine;
    private IEnumerator attackCoroutine;

    void Start() {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        stateMachine.AddState("Patrolling", new NormalPatrollingState(this));
        stateMachine.AddState("Following", new NormalFollowingState(this));
        stateMachine.AddState("Attacking", new NormalAttackingState(this));
        stateMachine.AddState("Stunned", new NormalStunnedState(this));
        stateMachine.ChangeState("Patrolling");
    }

    void Update() {
        if (inImmune) ChangeState("Patrolling");
        stateMachine.OnUpdate();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("TazerProjectile")) return;

        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        ChangeState("Stunned");
        other.gameObject.SetActive(false);
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
            attackCoroutine = Attack();
            StartCoroutine(attackCoroutine);
        } else {
            ChangeState("Following");
        }
    }

    IEnumerator Attack() {
        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(1);
        miscSource.PlayOneShot(attackAudio);
        if (SightCheck.IsInSight(transform, target.transform.position, attackSightDistance, attackSightCone)) {
            target.GetComponent<playerstats>().currentHP -= damage;
        }
        yield return new WaitForSeconds(2);
        ChangeState("Following");
        yield return null;
    }

    public void ChangeState(string stateName) {
        stateMachine.ChangeState(stateName);
    }

    [System.Serializable]
    public struct NormalEnemyGizmosSettings {
        public bool patrolCone;
        public bool followCone;
        public bool attackCone;
        public bool hearCone;
    }
}