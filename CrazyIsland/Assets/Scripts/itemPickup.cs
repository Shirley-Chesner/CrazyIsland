using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    private bool pickUpAllowed;
    playerInventory PlayerInventory = null;
    public item item;
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
        PlayerInventory.add(item);
        Destroy(gameObject);
    }
}
