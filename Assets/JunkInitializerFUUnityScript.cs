using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkInitializerFUUnityScript : MonoBehaviour
{

    public Camera mainCam;

	// Use this for initialization
	void Start()
    {
        GlobalController.instance.RegisterCamera(mainCam);
	}
	

	// Update is called once per frame
	void Update()
    {
        GlobalController.instance.Update();
    }
}
