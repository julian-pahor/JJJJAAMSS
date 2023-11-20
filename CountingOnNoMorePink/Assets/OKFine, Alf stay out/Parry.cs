using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Parry : MonoBehaviour
{
    bool parrying;
    float parryTime;

    bool attacked;

    float lateTimer;
    //  1/4 time in ms for 130 BPM   +a little extra  //462 = 1/4 for 130BPM
    private float beatMS = 0.462f;

    private float perfectWindow;

    private double inputTime;
    private double attackTime;

    public TextMeshProUGUI text;
    private ParryResult result;

    public float inputLag = 0.05f;

    public ParryReturn2 parryReturn2;

    private bool inTestingZone = true;

    public System.Action onParrySuccess;



    private enum ParryResult
    {
        Early,
        Perfect,
        Late,
        Miss
    }

    // Start is called before the first frame update
    void Start()
    {
        perfectWindow = beatMS * 0.35f;
        parrying = false;
        attacked = false;
        parryTime = 0;
        lateTimer = 0;

        if(Wobbit.instance != null)
        {
            inTestingZone = false;
        }

    }

    //exposing this so we can call it from player's input and not have button detections everywhere
    public void DoParry()
    {
        inputTime = Time.timeAsDouble;
             
        

        if(attacked)
        {
            ParryOutcome();
            lateTimer = 0;
            //attacked = false;
        }
        else
        {
            parryTime = 0;
            parrying = true;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        //-----
        //Parry flag for anticipated input timer
        //
        if (parrying)
        {
            parryTime += Time.deltaTime;
        }
        if (parryTime >= beatMS / 4f)
        {
            parrying = false;
            parryTime = 0;
        }
        //------

        //-----
        //Parry flag for input after parrycheck has been called
        //If player does not input before timer runs out they fail the parry
        if (attacked)
        {
            lateTimer += Time.deltaTime;
        }
        if (lateTimer >= beatMS / 2.5f)
        {
            attacked = false;
            lateTimer = 0;
            //Missed parry Take Damage
            ///Horrible Horrible absolutlely horrible chain of reference call to make player take damage
            if (!inTestingZone)
                Wobbit.instance.playerMovement.TakeDamage();
            Debug.Log("TAKE DAMAGE AHHH");
            result = ParryResult.Miss;
            Wobbit.instance.persistentData.currentSongMissedParrys += 1;
        }
        //-----

    }

    /// <summary>
    /// Function to be called by attack event which player has to parry
    /// </summary>
    public void ParryChance()
    {
        //TODO:
        //Check if player is on their input cooldown to immediately fail the parry
        attackTime = Time.timeAsDouble;

        if(parrying)
        {
            ParryOutcome();
        }
        else
        {
            attacked = true;
        }
    }

    private void ParryOutcome()
    {

        Wobbit.instance.persistentData.currentSongTotalParrys += 1;
        //TODO: Expose + visualise inputLag for better playtesting
        //Value is added to the time of the input to customise players sense of timing

        //Parrying flag for when player anticipates parried attack
        //(for both perfect + early checks)
        if(parrying)
        {
            double resultTime = attackTime - (inputTime + inputLag);
            if (resultTime <= perfectWindow / 2.4f)
            {
                result = ParryResult.Perfect;
                Wobbit.instance.persistentData.currentSongPerfectParrys += 1;
            }
            else if (resultTime <= beatMS / 4f)
            {
                result = ParryResult.Early;
            }
            ParryReturn();
            Debug.Log("ResultTime " + resultTime);
        }
        //Attacked flag for if player inputs after parried attack has already passed
        //(For both perfect + late checks)
        else if(attacked)
        {
            double resultTime = (inputTime + inputLag) - attackTime;

            if (resultTime <= perfectWindow / 2.4f)
            {
                result = ParryResult.Perfect;
                Wobbit.instance.persistentData.currentSongPerfectParrys += 1;
            }
            else if (resultTime <= beatMS / 2.5f)
            {
                result = ParryResult.Late;
            }
            ParryReturn();
            Debug.Log("ResultTime " + resultTime);
        }

        UpdateText();
        parrying = false;
        attacked = false;

    }

    //successful parry
    public void ParryReturn()
    {
        if(!inTestingZone)
        {
            Instantiate(parryReturn2, transform.position, Quaternion.identity);
            onParrySuccess?.Invoke();
        }
        else
        {
            Debug.Log("You should only be seeing this in the Input Testing Zone");
        }

    }

    void UpdateText()
    {
        if(text != null)
        {
            switch (result)
            {
                case (ParryResult.Early):
                    text.text = "Parry - Early";
                    break;
                case (ParryResult.Perfect):
                    text.text = "Parry - Perfect!";
                    break;
                case (ParryResult.Late):
                    text.text = "Parry - Late";
                    break;
                case (ParryResult.Miss):
                    text.text = "Parry - Miss";
                    break;
            }
        }
        else
        {
            switch (result)
            {
                case (ParryResult.Early):
                    Debug.Log("Parry - Early");
                    break;
                case (ParryResult.Perfect):
                    Debug.Log("Parry - Perfect!");
                    break;
                case (ParryResult.Late):
                    Debug.Log("Parry - Late");
                    break;
                case (ParryResult.Miss):
                    Debug.Log("Parry - Miss");
                    break;
            }
        }
        
    }
}
