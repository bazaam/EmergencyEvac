using System;
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
        ModifyRemainingMeeplesUI(1);
    }

    public void AlertExit(MeepleController2D meeple)
    {
        mMeeples.Remove(meeple);
        meeple.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
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
