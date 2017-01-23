using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GlobalController.instance.RegisterWall(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
