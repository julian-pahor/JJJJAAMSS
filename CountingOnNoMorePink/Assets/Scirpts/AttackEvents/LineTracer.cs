using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTracer : MonoBehaviour
{
    public List<Vector3> waypoints = new List<Vector3>();
    Vector3 currentWaypoint;
    Vector3 nextWaypoint;

    float timer;
    float travelTime;
    float decayTime;
    bool decaying;
    int index;

    public void Initialise(float travelTime, float decayTime)
    {
        this.travelTime = travelTime;
        this.decayTime = decayTime;

        //need at least two waypoints
        if (waypoints.Count <= 1)
        {
            decaying = true;
            return;
        }

        currentWaypoint = waypoints[0];
        nextWaypoint = waypoints[1];
    }

    private void Update()
    {
        timer += Time.deltaTime;

        //destroy object after decay finishes - ignore rest of update
        if(decaying)
        {
            if(timer >= decayTime)
            {
                Destroy(gameObject);
            }
            return;
        }

        float lerp = timer / travelTime;

        //finished travel - check for next waypoint
        if(lerp >= 1)
        {  
            //reset timer
            timer = 0;
            //set our position
            currentWaypoint = waypoints[index];
            //increase index
            index += 1;
            //check for end of list
            if(index >= waypoints.Count)
            {
                decaying = true;
                return;
            }
            //set next destination
            nextWaypoint = waypoints[index];
        }

        else
        {
            transform.position = Vector3.Lerp(currentWaypoint, nextWaypoint, lerp);
        }

        
    }

}
