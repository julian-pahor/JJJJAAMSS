using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    Vector3 target;
    Vector3 anchor1;
    Vector3 anchor2;
    Vector3 origin;
    float lifespan;
    float timer;

    
    public void Initialise(Vector3 origin, Vector3 anchor1, Vector3 anchor2, Vector3 target, float lifespan)
    {
        this.origin = origin;
        this.anchor1 = anchor1;
        this.anchor2 = anchor2;
        this.target = target;
        this.lifespan = lifespan;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.position = Utilities.CubicLerp(origin, anchor1,anchor2, target, timer / lifespan);
        if (timer >= lifespan)
            Destroy(gameObject);

        
    }
}
