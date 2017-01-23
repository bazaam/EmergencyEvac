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

        string timeRemaining = GameObject.Find("TimeRemaining").GetComponent<UnityEngine.UI.Text>().text;
        string[] timeRemainingSegs = timeRemaining.Split(':');

        int minutes = int.Parse(timeRemainingSegs[0]);
        int seconds = 0;
        if (timeRemainingSegs.Length > 0)
        {
            seconds = int.Parse(timeRemainingSegs[1]);
        }

        GlobalController.instance.SetLevelTime(minutes, seconds);
    }
	

	// Update is called once per frame
	void Update()
    {
        GlobalController.instance.Update();
    }
}
