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

    private int mMinX = int.MinValue;
    private int mMaxX = int.MaxValue;
    private int mMinY = int.MinValue;
    private int mMaxY = int.MaxValue;

    private Vector3 mExitCenter = new Vector3();

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

    public void TryToContainMeeple(MeepleController2D meeple)
    {
        Vector3 meepleCenter = meeple.gameObject.GetComponent<Collider>().bounds.center;

        Vector3 transform = new Vector3();
        if (meepleCenter.x < mMinX)
        {
            ++transform.x;
        }
        if (meepleCenter.x > mMaxX)
        {
            --transform.x;
        }
        if (meepleCenter.y < mMinY)
        {
            ++transform.y;
        }
        if (meepleCenter.y > mMaxY)
        {
            --transform.y;
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
    }

    public Vector3 GetExitCenter()
    {
        return mExitCenter;
    }

    public void AlertExit(MeepleController2D meeple)
    {
        mMeeples.Remove(meeple);
        meeple.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
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
        int wallMinX = (int)(wallBounds.center.x - wallBounds.extents.x);
        int wallMaxX = (int)(wallBounds.center.x + wallBounds.extents.x);
        int wallMinY = (int)(wallBounds.center.y - wallBounds.extents.y);
        int wallMaxY = (int)(wallBounds.center.y + wallBounds.extents.y);

        mMinX = Math.Max(mMinX, wallMinX);
        mMaxX = Math.Min(mMaxX, wallMaxX);
        mMinY = Math.Max(mMinY, wallMinY);
        mMaxY = Math.Min(mMaxY, wallMaxY);
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
