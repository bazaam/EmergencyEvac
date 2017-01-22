using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController
{
	public static GlobalController instance = new GlobalController();

	private List<MeepleController2D> mMeeples = new List<MeepleController2D>();

    GlobalController()
    {
        //
    }

    public void Update()
    {
        if (UnityEngine.Input.GetMouseButton(1))
        {
            foreach (MeepleController2D meeple in mMeeples)
            {
                meeple.AlertMeeple(UnityEngine.Input.mousePosition);
            }
        }
    }

    public void RegisterMeeple(MeepleController2D meeple)
    {
        mMeeples.Add(meeple);
    }
}
