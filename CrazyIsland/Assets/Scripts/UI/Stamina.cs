using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Image[] stamina;

    int currentStamina;

    private void Start()
    {
        currentStamina = stamina.Length;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentStamina--;
            stamina[currentStamina].enabled = false;
        }
    }
}
