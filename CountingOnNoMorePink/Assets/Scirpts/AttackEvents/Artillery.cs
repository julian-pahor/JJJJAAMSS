using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    Vector3 target;
    Vector3 anchor;
    Vector3 origin;
    float lifespan;
    float timer;

    
    public void Initialise(Vector3 origin, Vector3 anchor, Vector3 target, float lifespan)
    {
        this.origin = origin;
        this.anchor = anchor;
        this.target = target;
        this.lifespan = lifespan;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.position = Utilities.QuadraticLerp(origin, anchor, target, timer / lifespan);
        if (timer >= lifespan)
            Destroy(gameObject);

        
    }
}
