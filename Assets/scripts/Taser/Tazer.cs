
using UnityEngine;
using System.Collections;

public class Tazer : MonoBehaviour {
    public float fireCooldown;
    public GameObject prefab;
    public float offsetY;
    public float projectileSpeed;
    public float projectileLifetime;
    private AimBehaviourBasic aimBehaviour;
    private float lastShot = -1000f;

    void Start () {
        aimBehaviour = GetComponent<AimBehaviourBasic>();
    }

    void Update () {
        if (aimBehaviour.aim && Input.GetMouseButtonDown(0)) {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire() {
        if (Time.realtimeSinceStartup - lastShot < fireCooldown) yield break;
        lastShot = Time.realtimeSinceStartup;

        float fireTime = Time.realtimeSinceStartup;
        Vector3 forward = transform.forward;
        GameObject projectile = Instantiate(prefab);
        projectile.transform.position = transform.position + new Vector3(0, offsetY, 0) + forward;

        while (Time.realtimeSinceStartup - fireTime < projectileLifetime) {
            projectile.transform.position += forward * projectileSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(projectile);
    }
}
 