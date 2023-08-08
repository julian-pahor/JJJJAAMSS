using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbit : MonoBehaviour
{
    public static Wobbit instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("oh no another wobbit");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        timeline = FindObjectOfType<BeatTimeline>();
    }

    //very temporary references to junk I need for the demo

    public Bullit bulletFab;
    public BoomBlock zoneFab;
    public Transform bossOrigin;
    public Transform player;
    public GameObject warning;
    public GameObject pink;


    public Tracer tracer;
    public Transform hand1;
    public Transform hand2;


    public BeatTimeline timeline;

}
