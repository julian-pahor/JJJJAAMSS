using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    //Polygon poly;
    MeshRenderer mershRernderer;
    Material circleShader;

    float initial;
    float lifespan;
    float currentLife;
    bool direction;



    private void Start()
    {
        mershRernderer = GetComponent<MeshRenderer>();
        mershRernderer.enabled = false;
        circleShader = mershRernderer.material;
    }

    public void Initialise(float timer)
    {
        Debug.Log(timer);
        mershRernderer.enabled = true;
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
        //this lerp wasn't always pointless I swear
        currentLife -= Time.deltaTime;
        if (!direction)
            circleShader.SetFloat("_Inner", Mathf.Lerp(0, 1, currentLife / lifespan));
        else
            circleShader.SetFloat("_Inner", Mathf.Lerp(1, 0, currentLife / lifespan));

        if (currentLife <= 0)
            mershRernderer.enabled = false;
    }
}
