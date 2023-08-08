using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LerpTester : MonoBehaviour
{
    [Range(0f, 1f)]
    public float lerp;
    public float distance;
    public Transform start;
    public Vector3 a1;
    public Transform mid;
    public Vector3 a2;
    public Transform end;

    public Transform thing;


    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            GenerateAnchors(3);
        }

        if(lerp < 0.5f)
        {
            thing.transform.position = Utilities.QuadraticLerp(start.position, a1, mid.position, lerp / .5f);
        }
        else
        {
            thing.transform.position = Utilities.QuadraticLerp(mid.position, a2, end.position, (lerp-.5f) / .5f);
        }

    }

    void GenerateAnchors(float curviness)
    {
        Vector3 midpoint1 = Vector3.Lerp(start.position, mid.position, 0.5f);
        Vector3 midpoint2 = Vector3.Lerp(mid.position, end.position, 0.5f);

        Vector3 directionToMid = (mid.position - Vector3.Lerp(start.position, end.position, 0.5f)).normalized;

        a1 = midpoint1 + directionToMid * curviness;

        a2 = midpoint2 + directionToMid * curviness;
    }
}
