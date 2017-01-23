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

    private const float kMagnitudeToStartFear = 20.0f;
    private const float kMagnitudeToHaveAFearJump = 10.0f;
    private const float kSecondsOfFear = 3.0f;
    private const float kHeightOfFearMultiplier = 2.0f;
    private const float kSteepnessOfFearRecovery = 8.0f;

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
            float fearMagnified = (FearEasingRatio(mFearTimeRemaining) * kHeightOfFearMultiplier) + 1.0f;
            mFearTimeRemaining = Mathf.Max(mFearTimeRemaining - (Time.deltaTime / kSecondsOfFear), 0.0f);
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
            mMovementDirection = clickToMyPos; // magnitude / kMagnitudeToStartFear

            mFearTimeRemaining = magnitude < kMagnitudeToHaveAFearJump ? 0.2f : 0.0f;
        }
    }

    public void AlertExit()
    {
        GlobalController.instance.AlertExit(this);
    }

    private float FearEasingRatio(float time)
    {
        return 1.0f - (1.0f / Mathf.Pow(time + 1.0f, kSteepnessOfFearRecovery));
    }

    // Put Trigger Code Below This Point
    // This Line Is Written So Obtusely That Even Git Merge Can Figure It Out

    private void OnTriggerEnter(Collider inCollider)
    {
        if (inCollider.tag == "wall")
        {

            Vector3 mCenter = gameObject.GetComponent<Collider>().bounds.center;
            Vector2 mCenterV2 = new Vector2(mCenter.x, mCenter.y);
            Vector2 mDirectionV2 = new Vector2(mMovementDirection.x, mMovementDirection.y);


            Vector2 result = new Vector2();
            Vector2 colliderS1 = new Vector2(inCollider.bounds.center.x - inCollider.bounds.extents.x, inCollider.bounds.center.y - inCollider.bounds.extents.y);
            Vector2 colliderS2 = new Vector2(inCollider.bounds.center.x + inCollider.bounds.extents.x, inCollider.bounds.extents.y - inCollider.bounds.extents.x);
            Vector2 colliderP1 = mCenterV2 + (mDirectionV2 * 50.0f);
            Vector2 colliderP2 = mCenterV2 + (mDirectionV2 * -50.0f);


            if (Utility.LineSegmentsIntersectPos(ref result, colliderS1, colliderS2, colliderP1, colliderP2))
            {
                //
            } 
            
        }
    }

   
}
