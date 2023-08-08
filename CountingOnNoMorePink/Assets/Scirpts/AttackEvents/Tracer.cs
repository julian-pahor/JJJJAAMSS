using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Tracer : MonoBehaviour
{

    float maxLerp;
    float currentLerp;

    Vector3 start;    
    Transform anchor;
    Vector3 end;

    Vector3 a1;
    Vector3 a2;

    public void SetUp(Vector3 start, Vector3 end, Transform anchor, float curviness, float time)
    {
        

        this.start = start;
        this.anchor = anchor;
        this.end = end;
        maxLerp = time;
        GetComponentInChildren<TrailRenderer>().enabled = true;
        GenerateAnchors(curviness);
    }

    private void Update()
    {
        currentLerp += Time.deltaTime;
        float lerpercents = currentLerp / maxLerp;

        if (lerpercents < 0.5f)
        {
            transform.position = Utilities.QuadraticLerp(start, a1, anchor.position, lerpercents / .5f);
        }
        else
        {
            transform.position = Utilities.QuadraticLerp(anchor.position, a2, end, (lerpercents - .5f) / .5f);
        }



        //pool this thing
        if (currentLerp >= maxLerp * 4)
        {
            Destroy(gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(a1, 1);
        Gizmos.DrawWireSphere(a2, 1);
    }
    void GenerateAnchors(float curviness)
    {
        Vector3 midpoint1 = Vector3.Lerp(start, anchor.position, 0.5f);
        Vector3 midpoint2 = Vector3.Lerp(anchor.position, end, 0.5f);

        Vector3 directionToMid = (anchor.position - Vector3.Lerp(start, end, 0.5f)).normalized;

        a1 = midpoint1 + directionToMid * curviness;

        a2 = midpoint2 + directionToMid * curviness;
    }
}
