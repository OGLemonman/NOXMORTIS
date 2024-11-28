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
    public float chargeSpeed = 9f;
    public float chargeDuration = 2f;
    public float attackCooldown = 3f;
    public float damage = 70f;
    private float lastAttack;
    private Rigidbody rb;
    private StateMachine stateMachine;
    private IEnumerator attackCoroutine;
    private bool isCharging;

    void Start() {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        stateMachine.AddState("Patrolling", new ChargerPatrollingState(this));
        stateMachine.AddState("Attacking", new ChargerAttackingState(this));
        stateMachine.AddState("Stunned", new ChargerStunnedState(this));
        stateMachine.ChangeState("Patrolling");
    }

    void Update() {
        stateMachine.OnUpdate();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("TazerProjectile")) return;

        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        isCharging = false;
        ChangeState("Stunned");
        other.gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision) {
        if (!isCharging || !collision.gameObject.CompareTag("Player")) return;

        target.GetComponent<playerstats>().currentHP -= damage;
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
            attackCoroutine = Attack();
            StartCoroutine(attackCoroutine);
        } else {
            ChangeState("Patrolling");
        }
    }

    IEnumerator Attack() {
        animator.SetInteger("State", 0);

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 initialPos = transform.position + direction;
        transform.LookAt(new Vector3(initialPos.x, transform.position.y, initialPos.z));

        yield return new WaitForSeconds(0.5f);
        isCharging = true;
        animator.SetInteger("State", 1);

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < chargeDuration) {
            Vector3 newPos = transform.position + direction * chargeSpeed * Time.deltaTime;
            transform.position = newPos;
            transform.LookAt(new Vector3(newPos.x, transform.position.y, newPos.z));

            yield return null;
        }

        isCharging = false;
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