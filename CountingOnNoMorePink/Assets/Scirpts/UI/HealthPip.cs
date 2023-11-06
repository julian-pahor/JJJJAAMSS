using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//ALSO DEPRECATED
/*
public class HealthPip : MonoBehaviour
{

    public GameObject fillTracer;
    public float filltime;
    public Sprite fullSprite;
    public Sprite damageSprite;



    Transform tracerStart;
    Transform tracerAnchor;
    Image image;
    float timer;
    bool currentState;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (timer < 0)
            return;

        timer -= Time.deltaTime;
        fillTracer.transform.position = Utilities.QuadraticLerp(tracerStart.position, tracerAnchor.position, transform.position, 1 - (timer / filltime));
        fillTracer.transform.localScale = Vector3.Lerp(new Vector3(0.1f,0.1f,0.1f),Vector3.one,1 - (timer / filltime));
        if (timer <= 0)
        {
            SetStateImmediate();
        }
    }

    public void Initialise(HealthBar bar)
    {
        tracerAnchor = bar.tracerAnchor;
        tracerStart = bar.tracerStart;
    }

    public void SetState(bool full)
    {
        if (full)
        {
            if (!currentState)
            {
                currentState = full;
                StartEffect(filltime);
            }
        }
        else
        {
            currentState = full;
            SetStateImmediate();
        }
    }

    void SetStateImmediate()
    {
        fillTracer.SetActive(false);
        image.sprite = currentState ? fullSprite : damageSprite;
    }
    void StartEffect(float duration)
    {
        fillTracer.SetActive(true);
        fillTracer.transform.position = tracerStart.position;
        timer = filltime;
    }
   
}
*/
