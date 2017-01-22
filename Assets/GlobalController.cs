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

    GlobalController()
    {
        //
    }

    public void Update()
    {
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

    public void RegisterMeeple(MeepleController2D meeple)
    {
        mMeeples.Add(meeple);
    }

    public void RegisterCamera(Camera mainCam)
    {
        mCamera = mainCam;
    }
}
