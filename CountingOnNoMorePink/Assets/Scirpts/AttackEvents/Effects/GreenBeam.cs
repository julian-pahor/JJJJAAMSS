using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBeam : MonoBehaviour
{
    public LineRenderer beam;

    public float beamTime;
    public AnimationCurve beamCurve;

    public Transform player;
    public Transform target;

    float timer;

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

    [ContextMenu("TEST BEEM")]
    public void BeamOn()
    {
        timer = beamTime;
        beam.enabled = true;
    }
}
