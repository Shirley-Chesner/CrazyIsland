using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject thirdCam;
    public GameObject firstCam;
    public int camMode;

    void Update()
    {
        if (Input.GetButtonDown("Camera"))
        {
            if (camMode == 1) camMode = 0;
            else camMode = 1;
            StartCoroutine(camChange());
        }
    }

    IEnumerator camChange()
    {
        yield return new WaitForSeconds(0.01f);
        if (camMode == 0)
        {
            thirdCam.SetActive(true);
            firstCam.SetActive(false);
        } else
        {
            thirdCam.SetActive(false);
            firstCam.SetActive(true);
        }
    }
}
