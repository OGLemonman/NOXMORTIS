using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UseItem : MonoBehaviour
{
    public Transform interactableObject;
    public float interactionDistance = 3.0f;

    void Update()
    {
        // Check if the E key is pressed and if the player is close enough to the object
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, interactableObject.position) < interactionDistance)
        {
            Use();
        }
    }

    void Use()
    {
        Debug.Log("Interacting with the object!");
        // Add your interaction logic here, like opening a door, picking up an item, etc.
    }
}