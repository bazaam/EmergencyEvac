using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController
{
	public static GlobalController instance = new GlobalController();

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
                foreach (MeepleController2D meeple in mMeeples)
                {
                    meeple.AlertMeeple(UnityEngine.Input.mousePosition);
                }
            }
        }
    }

    public void RegisterMeeple(MeepleController2D meeple)
    {
        mMeeples.Add(meeple);
    }
}
