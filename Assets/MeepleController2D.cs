using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleController2D : MonoBehaviour {

    private float offsetForDebugging = 50.0f;

	// Use this for initialization
	void Start ()
    {
        Invoke("RegisterThis", 1.0f);
	}

    void RegisterThis()
    {
        GlobalController.instance.RegisterMeeple(this);
    }
	
	// Update is called once per frame
	void Update () {
        GlobalController.instance.Update();
	}
            
    public void AlertMeeple(Vector3 clickPosition)
    {
        offsetForDebugging *= -1;
        transform.Translate(new Vector3(offsetForDebugging, offsetForDebugging, 0.0f));
    }
}
