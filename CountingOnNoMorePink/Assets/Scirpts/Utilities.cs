using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Returns a point in a polar direction at a specified distance from our origin
    /// </summary>
    public static Vector2 PointWithPolarOffset(Vector2 origin, float distance, float angle)
    {
        Vector2 point;



        point.x = origin.x + Mathf.Sin((angle * Mathf.PI) / 180) * distance;
        point.y = origin.y + Mathf.Cos((angle * Mathf.PI) / 180) * distance;



        return point;
    }


    /// <summary>
    /// Returns a point in a polar direction at a specified distance from our origin
    /// </summary>
    public static Vector3 PointWithPolarOffset(Vector3 origin, float distance, float angle)
    { 
        Vector3 point;


        point.y = origin.y;
        point.x = origin.x + Mathf.Sin((angle * Mathf.PI) / 180) * distance;
        point.z = origin.z + Mathf.Cos((angle * Mathf.PI) / 180) * distance;



        return point;
    }

  

}
