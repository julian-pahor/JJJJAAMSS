using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.UI.Image;

//Used by attack events to create an area with a 'tell' that will become dangerous after a fixed number of beats
public class DelayedDangerZone : MonoBehaviour
{

    public int armBeats; //number of beats before we detonate
    public int activeBeats; //how long we remain dangerous for (usually just one beat);

    public ParticleSystem tellEffect;
    public ParticleSystem launchEffect;
    public CircleRenderer indicator;

    public bool beatLocked; //whether this is locked to the beat, or runs on a timer

    bool isActive; //if we're dangerous or not
    Collider col;
    Artillery artilleryTracer;

    //timers for if we're not on beat
    float armTime;
    float activeTime;
    float timer;
<<<<<<< HEAD
 
    bool isArmed;
    bool tracerLaunched;

=======

    float armStart;
    float activeStart;
 
    bool isArmed;
    bool tracerLaunched;

>>>>>>> SaveFeatureTestingV2
    //do this before initialising yeh that makes sense
    public void SetArtilleryTracer(Artillery tracer)
    {
        artilleryTracer = tracer;
    }
    public void InitialiseOnBeat(int armTime, int activeTime)
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Tick;
        beatLocked = true;
        this.armBeats = armTime;
        this.activeBeats = activeTime;
        col = GetComponent<Collider>();
        col.enabled = false;
       
        Tick(0, 0);
    }

    public void InitialiseOnTimer(float delay, float armTime, float activeTime)
    {
        
        beatLocked = false;
        timer = delay + armTime + activeTime + 1; //total time we're active for (plus 1 sec for particles)
        col = GetComponent<Collider>();
        col.enabled = false;

        //DO NOT QUESTION ME JULIAN
        this.armTime = armTime;
        this.activeTime = activeTime;

        armStart = armTime + activeTime + 1;
        activeStart = activeTime + 1;

    }
    //FIX YOUR MATHEMATICS ALFRED
    private void Update()
    {
        if (beatLocked)
            return;
        timer -= Time.deltaTime;

        if(!isActive)
        {
            //arm
<<<<<<< HEAD
            if(!tracerLaunched && timer <= (armTime - activeTime) *2)
            {
                StartTracer(armTime);
                tracerLaunched = true;
            }
            else if (!isArmed && timer <= armTime)
=======
            if(!tracerLaunched && timer <= armStart + (armTime*2))
            {
                StartIndicator(armTime*3);
                StartTracer(armTime);
                tracerLaunched = true;
            }
            else if (!isArmed && timer <= armStart)
>>>>>>> SaveFeatureTestingV2
            {
                tellEffect.Play(true);
                isArmed = true;
                //StartTracer(armTime- activeTime); 
            }

            else if(timer <= activeStart)
            {
                Activate();
            }

            
        }

        else
        {
           
            if(timer <= 0)
            {
                Destroy(gameObject);
            }
            else if(timer <= 1)
            {
                Deactivate(); //flag so we don't run this every frame
            }
            
        }

    }

    void Tick(int beat, int bar)
    {
        if(!isActive)
        { 
            
            if (armBeats <= 0)
            {
                Activate();
            }
            else if(armBeats == 1) //arm
            {
                tellEffect.Play(true);
                //(BeatBroadcast.instance.beatLength);
            }
            else if (armBeats == 2) //aUHHHHHHHUAHUAHG
            {
                //tellEffect.Play(true);
                StartTracer(BeatBroadcast.instance.beatLength);
            }
            armBeats -= 1;
            //change value after checks
            
            
        }
        else
        {
           activeBeats -= 1;
            if(activeBeats < 0)
            {
                Destroy(gameObject);
            }
            else if (activeBeats == 0)
            {
                Deactivate();
            }
             
      
        }
    }

    void Activate()
    {  
        tellEffect.gameObject.SetActive(false);
        launchEffect.Play(true);
        isActive = true;
        col.enabled = true;
    }
    //turns off collider, but keeps the object around for a beat to let the particles play
    void Deactivate()
    {
      
        col.enabled = false;
    }

    void StartTracer(float timing)
    {
        if (artilleryTracer == null)
            return;

        Vector3 launchPoint = Wobbit.instance.bossOrigin.position; //hmmmmm

        float distanceToCentre = Vector3.Distance(launchPoint, transform.position) / 2;
        Vector3 directionToTarget = new Vector3(transform.position.x - launchPoint.x, 0, transform.position.z - launchPoint.z).normalized;
        Vector3 anchorPoint = launchPoint + (distanceToCentre * directionToTarget);
        anchorPoint = new Vector3(anchorPoint.x, anchorPoint.y + 20, anchorPoint.z);

        Artillery effect = Instantiate(artilleryTracer, launchPoint, Quaternion.identity);
        effect.Initialise(launchPoint, anchorPoint, transform.position, timing);


    }

<<<<<<< HEAD
=======
    void StartIndicator(float time)
    {
        if (indicator == null)
            return;
        
        indicator.StartTimer(time);

    }

>>>>>>> SaveFeatureTestingV2
    private void OnDestroy()
    {
        if(beatLocked)
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Tick;
    }
}
