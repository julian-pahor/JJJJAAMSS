using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    Polygon poly;
    MeshRenderer mershRernderer;

    float initial;
    float lifespan;
    float currentLife;
    bool direction;



    private void Start()
    {
        poly = GetComponent<Polygon>();
        mershRernderer = poly.GetComponent<MeshRenderer>();
        mershRernderer.enabled = false;
    }

    public void Initialise(float timer)
    {
        mershRernderer.enabled = true;
        initial = poly.polygonRadius;
        lifespan = timer;
        currentLife = timer;
    }

    public void Initialise(float timer, bool overrideDirection)
    {
        direction = overrideDirection;
        Initialise(timer);
    }


    void Update()
    {
        currentLife -= Time.deltaTime;
        if(!direction)
        poly.centerRadius = Mathf.Lerp(0,initial,currentLife/lifespan);
        else
        poly.centerRadius = Mathf.Lerp(initial, 0, currentLife / lifespan);

        if (currentLife <= 0)
            mershRernderer.enabled = false;
    }
}
