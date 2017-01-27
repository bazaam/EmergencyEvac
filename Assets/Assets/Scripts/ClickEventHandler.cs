using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEventHandler : MonoBehaviour
{

    public GameObject mouseDownParticle;
    public Camera mCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(mCamera.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1));
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 translatedPoint = new Vector3(hitInfo.point.x, hitInfo.point.y, 1);
                Instantiate(mouseDownParticle, translatedPoint, Quaternion.identity);

            }
        }
    }

}
