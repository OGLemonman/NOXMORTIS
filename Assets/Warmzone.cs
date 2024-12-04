using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmzone : MonoBehaviour
{
    public playerstats playerstats;

    public void Start()
    {
        playerstats = FindAnyObjectByType<playerstats>();
    }
    public void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerstats.currentTemperature += 5 * Time.deltaTime;
        }

    }

}
