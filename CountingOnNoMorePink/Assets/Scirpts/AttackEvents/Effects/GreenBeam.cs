using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class GreenBeam : MonoBehaviour
{
    public LineRenderer beam;

    public float beamTime;
    public AnimationCurve beamCurve;

    public Transform player;
    public Transform target;

    public VisualEffect pillarBoom;
    public VisualEffect parryBoom;
    public Transform anchor;

    float timer;

    private void Start()
    {
        pillarBoom.Stop();
    }
    private void Update()
    {
        if (timer <= 0)
            return;

        beam.widthMultiplier = beamCurve.Evaluate(timer/beamTime);

        beam.SetPosition(0, transform.position);
        beam.SetPosition(1, target.position);

        timer -= Time.deltaTime;

        if (timer <= 0)
            beam.enabled = false;
    }

    public void BeamOn()
    {
        timer = beamTime;
        beam.enabled = true;
        
        anchor.LookAt(transform.position);
 
        parryBoom.Play();
        pillarBoom.Play();
    }

}
