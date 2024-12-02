using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {
    public Transform[] spawns;
    public MobWeightGroup[] mobWeightGroup;
    public GameObject normalPrefab;
    public GameObject slowPrefab;
    public GameObject chargerPrefab;
    public Transform target;
    private List<GameObject> aliveMobs = new List<GameObject>();

    public void GenerateMob(int day) {
        MobWeightGroup weightGroup = mobWeightGroup[day-1];
        List<WeightRegion> weightRegions = new List<WeightRegion>();
        float totalWeight = 0f;

        // Populate weight regions
        if (weightGroup.normalMob > 0) {
            weightRegions.Add(new WeightRegion(0, totalWeight + weightGroup.normalMob, normalPrefab));
            totalWeight += weightGroup.normalMob;
        }

        if (weightGroup.slowMob > 0) {
            weightRegions.Add(new WeightRegion(totalWeight, totalWeight + weightGroup.slowMob, slowPrefab));
            totalWeight += weightGroup.slowMob;
        }

        if (weightGroup.chargerMob > 0) {
            weightRegions.Add(new WeightRegion(totalWeight, totalWeight + weightGroup.chargerMob, chargerPrefab));
            totalWeight += weightGroup.chargerMob;
        }

        // Choose random weight and find mob
        float value = Random.Range(0f, totalWeight);
        GameObject mob = weightRegions[0].mobPrefab;

        foreach (WeightRegion weightRegion in weightRegions) {
            if (value >= weightRegion.minValue && value < weightRegion.maxValue) {
                mob = weightRegion.mobPrefab;
                break;
            }
        }

        // Choose random spawn point
        int spawnValue = Random.Range(0, spawns.Length);
        Transform spawn = spawns[spawnValue];

        if (mob.name == "NormalEnemy") {
            mob.GetComponent<NormalEnemyController>().target = target;
        } else if (mob.name == "BeefyEnemy") {
            mob.GetComponent<SlowEnemyController>().target = target;
        } else if (mob.name == "ChargerEnemy") {
            mob.GetComponent<ChargerEnemyController>().target = target;
        }

        // Instantiate mob
        GameObject newMob = Instantiate(mob);
        aliveMobs.Add(newMob);

        Vector3 offset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        newMob.transform.position = spawn.position + offset;
    }

    public void DespawnMobs() {
        foreach (GameObject mob in aliveMobs) {
            Destroy(mob);
        }

        aliveMobs.Clear();
    }

    void OnValidate() {
        for (int i = 0; i < mobWeightGroup.Length; i++) {
            MobWeightGroup weightGroup = mobWeightGroup[i];
            weightGroup.normalMob = Mathf.Max(0, weightGroup.normalMob);
            weightGroup.slowMob = Mathf.Max(0, weightGroup.slowMob);
            weightGroup.chargerMob = Mathf.Max(0, weightGroup.chargerMob);
            mobWeightGroup[i] = weightGroup;
        }
    }

    [System.Serializable]
    public struct MobWeightGroup {
        public float normalMob;
        public float slowMob;
        public float chargerMob;
    }

    private struct WeightRegion {
        public float minValue;
        public float maxValue;
        public GameObject mobPrefab;

        public WeightRegion(float minValue, float maxValue, GameObject mobPrefab) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.mobPrefab = mobPrefab;
        }
    }
}