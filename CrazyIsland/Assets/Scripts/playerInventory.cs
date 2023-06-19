using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
    // In the future we will add variable for each item that the player can collect
  public int numberOfItems { get; private set; }

    public void itemCollected()
    {
        numberOfItems++;
    }
}
