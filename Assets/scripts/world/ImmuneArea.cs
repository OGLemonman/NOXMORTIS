
using UnityEngine;

public class ImmuneArea : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "NormalEnemy(Clone)") {
            other.gameObject.GetComponent<NormalEnemyController>().inImmune = true;
        } else if (other.gameObject.name == "BeefyEnemy(Clone)") {
            other.gameObject.GetComponent<SlowEnemyController>().inImmune = true;
        } else if (other.gameObject.name == "ChargerEnemy(Clone)") {
            other.gameObject.GetComponent<ChargerEnemyController>().inImmune = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "NormalEnemy(Clone)") {
            other.gameObject.GetComponent<NormalEnemyController>().inImmune = false;
        } else if (other.gameObject.name == "BeefyEnemy(Clone)") {
            other.gameObject.GetComponent<SlowEnemyController>().inImmune = false; 
        } else if (other.gameObject.name == "ChargerEnemy(Clone)") {
            other.gameObject.GetComponent<ChargerEnemyController>().inImmune = false;
        }
    }
}
