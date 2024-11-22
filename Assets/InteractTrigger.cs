using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "fabric")
        {
            Debug.Log("gettingfabric");
        }

        if(collider.gameObject.tag == "stone")
        {
            Debug.Log("gettingstone");
        }
        if(collider.gameObject.tag == "wood")
        {
            Debug.Log("gettingwood");
        }
        if(collider.gameObject.tag == "metal")
        {
            Debug.Log("gettingmetal");
        }
        if(collider.gameObject.tag == "food")
        {
            Debug.Log("Get food");
            //increase hunger
        }

        if(collider.gameObject.tag == "water")
        {
            Debug.Log("Get water");
            //increase thirst
        }
    }
}
