using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitTrigger : MonoBehaviour
{
    //recognize when the meeple runs into the ending

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meeple")
        {
            MeepleController2D meeple = other.gameObject.GetComponent<MeepleController2D>();
            meeple.AlertExit();
        }
    }
    //when the meeples run into the ending they are "saved" and removed. 
}
