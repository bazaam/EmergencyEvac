using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfximpacttrigger : MonoBehaviour
{
    AudioSource AudioPlayer;
	// Use this for initialization
	void Start ()
    {
        AudioPlayer = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		 
	}
    void OnTriggerEnter(Collider other)
    {
        // When a meeple hits a wall play MeepleWallImpact sound effect once
        if(other.tag == "Meeple")
        {
            AudioPlayer.Play();
        }
    }
}
