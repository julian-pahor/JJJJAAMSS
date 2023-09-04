using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CircleRenderer : MonoBehaviour
{

    public int points;
    public float outerRadius;
    public LineRenderer innerRenderer;
    public LineRenderer outerRenderer;

    bool active;
    float lifeSpan;
    float timer;
    
    float innerRadius;

    // Start is called before the first frame update
    void Start()
    {
        outerRenderer.positionCount = points + 1;
        outerRenderer.SetPositions(GeneratePoints(outerRadius));
    }

    public void StartTimer(float life)
    {
       
        //lineRenderer.enabled = true;
       
        lifeSpan = life;
        active = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (points <= 0)
            return;
        
        if(active)
        {
            timer += Time.deltaTime;
            innerRadius = Mathf.Lerp(0.1f,outerRadius, timer / lifeSpan);
            if(timer >= lifeSpan)
            {
                innerRenderer.enabled = false;
                outerRenderer.enabled = false;
            }
        }

       innerRenderer.positionCount = points+1;
       innerRenderer.SetPositions(GeneratePoints(innerRadius));
    }


    Vector3[] GeneratePoints(float radius)
    {
       

        Vector3[] allPoints = new Vector3[points +1];

        if (points <= 0) { points = 1; } //stop it dividing by 0
        float angleStep = (360f / points); //divide total arc of fire by number of shots

        Vector3 launchvector = Utilities.PointWithPolarOffset(transform.position, 1, 0) - transform.position;

        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;

        if (rotation < 0) { rotation = 360 + rotation; } //fixes negative degrees

        for (var i = 0; i < points; ++i)
        {
           
            
            float rotationO = rotation;
            rotationO = rotation / 2 + angleStep / 2;
            rotationO += angleStep * i;


            Vector3 nextPoint = Utilities.PointWithPolarOffset(transform.position, radius, rotationO);
            allPoints[i] = nextPoint;
            
        }
        allPoints[allPoints.Length -1] = allPoints[0];
        return allPoints;
    }
}
