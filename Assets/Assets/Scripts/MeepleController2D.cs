﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleController2D : MonoBehaviour
{
    // Our Screen Limits:
    // x: -23 to 21
    // y: -11 to 15.5
    // d:  51.364

    private enum CollisionResult
    {
        kNone,
        kFlipX,
        kFlipY,
        kReverse
    }

    // within "10" of the unit, it starts to move

    private const float kMagnitudeToStartFear = 0.3f;
    private const float kMagnitudeToHaveAFearJump = 0.1f;
    private const float kSecondsOfFear = 3.0f;
    private const float kHeightOfFearMultiplier = 2.0f;
    private const float kSteepnessOfFearRecovery = 8.0f;

    // Think of this as their "intelligence rate"
    // 2.0f is intelligent, 0.3f is "subtle intelligent", and 20.0f is "fuck you I'm getting outside"
    private const float kRotateTowardsExitRadsRate = 0.3f;
    private const float kRotateTowardsExitRadsRateAfterReversing = 4.0f;

    private float mSpeed = 12.0f;
    private float mFearTimeRemaining = 0.0f;
    private bool mMoving = false;
    private Vector3 mMovementDirection = new Vector3();
    
    private CollisionResult mCollisionResult = CollisionResult.kNone;
    private Vector2 mNearestRelevantCollisionPoint = new Vector2();
    

    // Use this for initialization
    void Start()
    {
        GlobalController.instance.RegisterMeeple(this);
    }

    // Update is run once per frame
    void Update()
    {
        //
    }

	void FixedUpdate()
    {
        float rotationRate = kRotateTowardsExitRadsRate;

        if (mCollisionResult != CollisionResult.kNone)
        {
            if (mCollisionResult == CollisionResult.kFlipX)
            {
                mMovementDirection.x *= -1;

            }
            else if (mCollisionResult == CollisionResult.kFlipY)
            {
                mMovementDirection.y *= -1;
            }
            else if(mCollisionResult == CollisionResult.kReverse)
            {
                mMovementDirection *= -1;
                rotationRate = kRotateTowardsExitRadsRateAfterReversing;
            }
            mCollisionResult = CollisionResult.kNone;
        }

        if (mMoving)
        {
            float fearMagnified = (FearEasingRatio(mFearTimeRemaining) * kHeightOfFearMultiplier) + 1.0f;
            mFearTimeRemaining = Mathf.Max(mFearTimeRemaining - (Time.deltaTime / kSecondsOfFear), 0.0f);
            Vector3 translateVector = mMovementDirection * (mSpeed * Time.deltaTime * fearMagnified);
            transform.Translate(translateVector);

            Vector3 myPosToExit = GlobalController.instance.GetExitCenter() - transform.position;
            mMovementDirection = Vector3.RotateTowards(mMovementDirection, myPosToExit, Time.deltaTime * rotationRate, 0.0f);
            mMovementDirection.z = 0;
            mMovementDirection.Normalize();
        }

        GlobalController.instance.TryToContainMeeple(this);
    }
            
    public void AlertMeeple(Vector3 clickPosition)
    {
        Vector3 clickToMyPos = transform.position - clickPosition;
        clickToMyPos.z = 0;
        float magnitude = clickToMyPos.magnitude;

        float magnitudeToStartFear = kMagnitudeToStartFear * GlobalController.instance.GetWidestPartOfLevel();

        if(magnitude < magnitudeToStartFear)
        {
            clickToMyPos.Normalize();

            mMoving = true;
            mMovementDirection = clickToMyPos; // magnitude / kMagnitudeToStartFear

            float magnitudeToFearJump = kMagnitudeToHaveAFearJump * GlobalController.instance.GetWidestPartOfLevel();

            mFearTimeRemaining = magnitude < magnitudeToFearJump ? 0.2f : 0.0f;
        }
    }

    public void AlertExit()
    {
        mMoving = false;
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
        if (inCollider.tag == "Wall")
        {
            Vector3 center = gameObject.GetComponent<Collider>().bounds.center;
            Vector2 centerV2 = new Vector2(center.x, center.y);
            Vector2 directionV2 = new Vector2(mMovementDirection.x, mMovementDirection.y);
            
            Vector2 result = new Vector2();
            Vector2 colliderBL = new Vector2(inCollider.bounds.center.x - inCollider.bounds.extents.x, inCollider.bounds.center.y - inCollider.bounds.extents.y);
            Vector2 colliderBR = new Vector2(inCollider.bounds.center.x + inCollider.bounds.extents.x, inCollider.bounds.center.y - inCollider.bounds.extents.y);
            Vector2 colliderTL = new Vector2(inCollider.bounds.center.x - inCollider.bounds.extents.x, inCollider.bounds.center.y + inCollider.bounds.extents.y);
            Vector2 colliderTR = new Vector2(inCollider.bounds.center.x + inCollider.bounds.extents.x, inCollider.bounds.center.y + inCollider.bounds.extents.y);

            Vector2 meepleP1 = centerV2 + (directionV2 * 50000.0f);
            Vector2 meepleP2 = centerV2 + (directionV2 * -50000.0f);


            if (Utility.LineSegmentsIntersectPos(ref result, colliderBL, colliderBR, meepleP1, meepleP2))
            {
                TestCollision(result, centerV2, CollisionResult.kFlipY);
            }

            if (Utility.LineSegmentsIntersectPos(ref result, colliderBL, colliderTL, meepleP1, meepleP2))
            {
                TestCollision(result, centerV2, CollisionResult.kFlipX);
            }
            
            if (Utility.LineSegmentsIntersectPos(ref result, colliderTL, colliderTR, meepleP1, meepleP2))
            {
                TestCollision(result, centerV2, CollisionResult.kFlipY);
            }
            
            if (Utility.LineSegmentsIntersectPos(ref result, colliderBR, colliderTR, meepleP1, meepleP2))
            {
                TestCollision(result, centerV2, CollisionResult.kFlipX);
            }

            //if(mCollisionResult == CollisionResult.kNone)
            //{
            //    mCollisionResult = CollisionResult.kReverse;
            //}
        }
    }

    private static int NumColl = 0;

    private void TestCollision(Vector2 newCollision, Vector2 centerV2, CollisionResult flip)
    {
        // We're currently not colliding with anything
        if(flip == CollisionResult.kNone)
        {
            mNearestRelevantCollisionPoint = newCollision;
            mCollisionResult = flip;
            return;
        }
        
        // We've already found a nearest collision, let's see if the new collision is closer
        Vector2 fromCenterToNewCollision = newCollision - centerV2;
        Vector2 fromCenterToLastCollision = mNearestRelevantCollisionPoint - centerV2;

        if(fromCenterToNewCollision.sqrMagnitude < fromCenterToLastCollision.sqrMagnitude)
        {
            mNearestRelevantCollisionPoint = newCollision;
            mCollisionResult = flip;
        }
    }


}
