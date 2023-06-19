using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodBoard : MonoBehaviour
{
    private bool pickUpAllowed;
    playerInventory PlayerInventory = null;
    private void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            pickUp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory = other.GetComponent<playerInventory>();
        if (PlayerInventory != null)
        {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory = other.GetComponent<playerInventory>();
        if (PlayerInventory != null)
        {
            pickUpAllowed = false;
        }
    }

    void pickUp()
    {
        PlayerInventory.itemCollected();
        Destroy(gameObject);
    }
}
