using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleController2D : MonoBehaviour {

    private float mSpeed = 10.0f;
    private float mFearMagnitude = 0.0f;
    private bool mMoving = false;
    private Vector3 mMovementDirection = new Vector3();

	// Use this for initialization
	void Start ()
    {
        GlobalController.instance.RegisterMeeple(this);
    }
	
	// Update is run once per frame
	void Update ()
    {
        if (mMoving)
        {
            Vector3 translateVector = mMovementDirection * (mSpeed * Time.deltaTime);
            transform.Translate(translateVector);
        }
    }
            
    public void AlertMeeple(Vector3 clickPosition)
    {
        Vector3 clickToMyPos = transform.position - clickPosition;
        mFearMagnitude = clickToMyPos.magnitude;
        clickToMyPos.Normalize();

        mMovementDirection = clickToMyPos;
        mMoving = true;
    }
}
