using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static bool LineSegmentsIntersectPos(ref Vector2 result, Vector2 pA1, Vector2 pA2, Vector2 pB1, Vector2 pB2)
    {
        float s1_x, s1_y, s2_x, s2_y;

        s1_x = pA2.x - pA1.x;
        s1_y = pA2.y - pA1.y;
        s2_x = pB2.x - pB1.x;
        s2_y = pB2.y - pB1.y;

        float s, t;
        s = (-s1_y * (pA1.x - pB1.x) + s1_x * (pA1.y - pB1.y)) / (-s2_x * s1_y + s1_x * s2_y);
        t = (s2_x * (pA1.y - pB1.y) - s2_y * (pA1.x - pB1.x)) / (-s2_x * s1_y + s1_x * s2_y);

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            result = new Vector2(pA1.x + (t * s1_x), pA1.y + (t * s1_y));
            return true;
        }

        return false; // No collision

    }

    public static bool LineIntersectionPoint(ref Vector2 result, Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
        {
            return false;
        }

        // now return the Vector2 intersection point
        result = new Vector2((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta);
        return true;
    }
}