﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController
{
	public static GlobalController instance = new GlobalController();

    private Camera mCamera;

	private List<MeepleController2D> mMeeples = new List<MeepleController2D>();

    private bool mLastLeftClickStateWasDown = false;

    private int mExistingMeeples = 0;
    private int mSavedMeeples = 0;
    private int mKilledMeeples = 0;

    private float mMinX = float.MaxValue;
    private float mMaxX = float.MinValue;
    private float mMinY = float.MaxValue;
    private float mMaxY = float.MinValue;

    private float mWidestPartOfLevel = 10.0f;

    private Vector3 mExitCenter = new Vector3();

    private bool mStarted = false;
    private float mTimeLeft = 10.0f;

    GlobalController()
    {
        //
    }

    public void SetLevelTime(int minutes, int seconds)
    {
        mTimeLeft = (float)(minutes * 60 + seconds);
    }

    public void Update()
    {
        ModifyRemainingTimeUI(Time.deltaTime);

        bool currentLeftClick = UnityEngine.Input.GetMouseButton(0);
        if (mLastLeftClickStateWasDown != currentLeftClick)
        {
            mLastLeftClickStateWasDown = currentLeftClick;

            if (currentLeftClick == true)
            {
                RaycastHit hitInfo;
                Ray ray = new Ray(mCamera.ScreenToWorldPoint(Input.mousePosition), new Vector3(0,0,1));
                if(Physics.Raycast(ray, out hitInfo))
                {
                    Vector3 translatedPoint = new Vector3(hitInfo.point.x, hitInfo.point.y, 0);
                    foreach (MeepleController2D meeple in mMeeples)
                    {
                        meeple.AlertMeeple(translatedPoint);
                    }
                }
            }
        }
    }

    public void TryToContainMeeple(MeepleController2D meeple)
    {
        Vector3 meepleCenter = meeple.gameObject.GetComponent<Collider>().bounds.center;

        Vector3 transform = new Vector3();
        if (meepleCenter.x < mMinX)
        {
            transform.x += 0.5f;
        }
        if (meepleCenter.x > mMaxX)
        {
            transform.x -= 0.5f;
        }
        if (meepleCenter.y < mMinY)
        {
            transform.y += 0.5f;
        }
        if (meepleCenter.y > mMaxY)
        {
            transform.y -= 0.5f;
        }

        meeple.transform.Translate(transform);
    }

    public void RegisterExit(LevelExitTrigger exit)
    {
        mExitCenter = exit.gameObject.GetComponent<Collider>().bounds.center;
    }

    public void RegisterMeeple(MeepleController2D meeple)
    {
        mMeeples.Add(meeple);
        ModifyRemainingMeeplesUI(1);

        mStarted = true;
    }

    public Vector3 GetExitCenter()
    {
        return mExitCenter;
    }

    public float GetWidestPartOfLevel()
    {
        return mWidestPartOfLevel;
    }

    public void AlertExit(MeepleController2D meeple)
    {
        mMeeples.Remove(meeple);
        meeple.gameObject.SetActive(false);
        ModifyRemainingMeeplesUI(-1);
        ModifySavedMeeplesUI(1);

        if (mExistingMeeples == 0)
        {
            LevelManager.instance.NextLevel();
        }
    }

    public void RegisterCamera(Camera mainCam)
    {
        mCamera = mainCam;
    }

    public void RegisterWall(Wall wall)
    {
        Bounds wallBounds = wall.gameObject.GetComponent<Collider>().bounds;
        float wallMinX = (wallBounds.center.x - wallBounds.extents.x + 1.0f);
        float wallMaxX = (wallBounds.center.x + wallBounds.extents.x - 1.0f);
        float wallMinY = (wallBounds.center.y - wallBounds.extents.y + 1.0f);
        float wallMaxY = (wallBounds.center.y + wallBounds.extents.y - 1.0f);

        mMinX = Math.Min(mMinX, wallMinX);
        mMaxX = Math.Max(mMaxX, wallMaxX);
        mMinY = Math.Min(mMinY, wallMinY);
        mMaxY = Math.Max(mMaxY, wallMaxY);

        mWidestPartOfLevel = Math.Max((mMaxX - mMinX), (mMaxY - mMinY));
    }

    private void ModifyRemainingTimeUI(float deltaTime)
    {
        mTimeLeft -= Time.deltaTime;

        double minutesF = Math.Floor((double)(mTimeLeft / 60.0f));
        int seconds = (int)Math.Floor(mTimeLeft - (minutesF * 60.0f));
        int minutes = (int)minutesF;

        string secondsString = seconds.ToString();
        if (secondsString.Length == 1)
        {
            secondsString = "0" + secondsString;
        }

        GameObject.Find("TimeRemaining").GetComponent<UnityEngine.UI.Text>().text = minutes.ToString() + ":" + secondsString;
    }

    private void ModifyRemainingMeeplesUI(int offset)
    {
        mExistingMeeples += offset;

        GameObject.Find("MeepleRemainingNum").GetComponent<UnityEngine.UI.Text>().text = mExistingMeeples.ToString();
    }
    private void ModifySavedMeeplesUI(int offset)
    {
        mSavedMeeples += offset;

        GameObject.Find("MeepleSavedNum").GetComponent<UnityEngine.UI.Text>().text = mSavedMeeples.ToString();
    }
}
