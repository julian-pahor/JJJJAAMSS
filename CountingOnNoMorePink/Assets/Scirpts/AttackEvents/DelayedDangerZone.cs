using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Used by attack events to create an area with a 'tell' that will become dangerous after a fixed number of beats
public class DelayedDangerZone : MonoBehaviour
{

    public int armBeats; //number of beats before we detonate
    public int activeBeats; //how long we remain dangerous for (usually just one beat);

    public ParticleSystem tellEffect;
    public ParticleSystem launchEffect;

    public bool beatLocked; //whether this is locked to the beat, or runs on a timer

    bool isActive; //if we're dangerous or not
    Collider col;

    //timers for if we're not on beat
    float armTime;
    float activeTime;
    float timer;
    bool isArmed;

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
        this.armTime = armTime + activeTime + 1;
        this.activeTime = activeTime + 1;

    }

    private void Update()
    {
        if (beatLocked)
            return;
        timer -= Time.deltaTime;

        if(!isActive)
        {
            if(!isArmed && timer <= armTime)
            {
                tellEffect.Play(true);
                isArmed = true;
            }

            else if(timer <= activeTime)
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
            else if(armBeats == 1)
            {
                tellEffect.Play(true);
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

    private void OnDestroy()
    {
        if(beatLocked)
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Tick;
    }
}
