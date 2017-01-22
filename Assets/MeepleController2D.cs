using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleController2D : MonoBehaviour
{
    // Our Screen Limits:
    // x: -23 to 21
    // y: -11 to 15.5
    // d:  51.364


    // within "10" of the unit, it starts to move

    private const float kMagnitudeToStartFear = 10.0f;
    private const float kSecondsOfFear = 3.0f;

    private float mSpeed = 5.0f;
    private float mFearTimeRemaining = 0.0f;
    private bool mMoving = false;
    private Vector3 mMovementDirection = new Vector3();

	// Use this for initialization
	void Start()
    {
        GlobalController.instance.RegisterMeeple(this);
    }
	
	// Update is run once per frame
	void Update()
    {
        if (mMoving)
        {
            float fearMagnified = FearEasingRatio(mFearTimeRemaining / kSecondsOfFear) + 1.0f;
            mFearTimeRemaining = Mathf.Max(mFearTimeRemaining - Time.deltaTime, 0.0f);
            Vector3 translateVector = mMovementDirection * (mSpeed * Time.deltaTime * fearMagnified);
            transform.Translate(translateVector);
        }
    }
            
    public void AlertMeeple(Vector3 clickPosition)
    {
        Vector3 clickToMyPos = transform.position - clickPosition;
        clickToMyPos.z = 0;
        float magnitude = clickToMyPos.magnitude;

        if(magnitude < kMagnitudeToStartFear)
        {
            clickToMyPos.Normalize();

            mMoving = true;
            mMovementDirection = clickToMyPos;
            mFearTimeRemaining = magnitude / kMagnitudeToStartFear * kSecondsOfFear;
        }
    }

    private float FearEasingRatio(float time)
    {
        // Quad In/Out
        const float dH = 0.5f; // Half duration
        const float cH = 0.5f; // Half rate of change
        time /= dH;
        if (time < 1) return cH / 2 * time * time * time * time;
        time -= 2;
        return -cH / 2 * (time * time * time * time - 2);
    }

    // Put Trigger Code Below This Point
    // This Line Is Written So Obtusely That Even Git Merge Can Figure It Out
}
