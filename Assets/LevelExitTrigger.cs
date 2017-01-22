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

            LevelManager.instance.NextLevel();
        }
    }
    //when the meeples run into the ending they are "saved" and removed. 
}
