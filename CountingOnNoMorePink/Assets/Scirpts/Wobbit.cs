using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public void GoToEditor()
    {
        SceneManager.LoadScene("UITesting");
    }

    //very temporary references to junk I need for the demo

    public Bullet bulletFab;
    public BoomBlock zoneFab;
    public Transform bossOrigin;
    public Transform player;
    public GameObject warning;
    public GameObject pink;

    public LineTracer lineTracer;

    public Tracer tracer;
    public GameObject tracerWindup;
    public Transform hand1;
    public Transform hand2;


    public Orbiter orbiterPrefab;

    public BeatTimeline timeline;

    public DelayedDangerZone delayedDangerZoneTest;

}
