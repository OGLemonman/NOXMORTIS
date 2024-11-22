using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{


    public playerstats playerstats;
    public PlayerResources playerResources;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "fabric")
        {
            Debug.Log("gettingfabric");
            playerResources.playerFabric += 10;
            Destroy(collider.gameObject);
        }

        if(collider.gameObject.tag == "stone")
        {
            Debug.Log("gettingstone");
            playerResources.playerStone += 10;
        }
        if(collider.gameObject.tag == "wood")
        {
            Debug.Log("gettingwood");
            playerResources.playerWood += 10;

        }
        if(collider.gameObject.tag == "metal")
        {
            Debug.Log("gettingmetal");
             playerResources.playerMetal += 10;
        }
        if(collider.gameObject.tag == "food")
        {
            Debug.Log("Get food");
            //increase hunger
            playerstats.currentHunger += 25;
        }

        if(collider.gameObject.tag == "water")
        {
            Debug.Log("Get water");
            //increase thirst
            playerstats.currentWater += 25;
        }
    }
}
