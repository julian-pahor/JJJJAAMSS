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
    private float beatMS = 0.4f;

    private float perfectWindow;

    private double inputTime;
    private double attackTime;

    public TextMeshProUGUI text;
    private ParryResult result;

    public float inputLag = 0.05f;

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

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
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


        //-----
        //Parry flag for anticipated input timer
        //
        if (parrying)
        {
            parryTime += Time.deltaTime;
        }
        if (parryTime >= beatMS / 2.75)
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
        if (lateTimer >= beatMS / 1.8)
        {
            attacked = false;
            lateTimer = 0;
            //Missed parry Take Damage
            Debug.Log("TAKE DAMAGE AHHH");
            result = ParryResult.Miss;
        }
        //-----

        UpdateText();
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
        //TODO: Expose + visualise inputLag for better playtesting
        //Value is added to the time of the input to customise players sense of timing

        //Parrying flag for when player anticipates parried attack
        //(for both perfect + early checks)
        if(parrying)
        {
            double resultTime = attackTime - (inputTime + inputLag);
            if(resultTime <= perfectWindow / 2.4)
            {
                result = ParryResult.Perfect;
            }
            else if(resultTime <= beatMS / 2.75)
            {
                result = ParryResult.Early;
            }
        }
        //Attacked flag for if player inputs after parried attack has already passed
        //(For both perfect + late checks)
        else if(attacked)
        {
            double resultTime = (inputTime + inputLag) - attackTime;

            if (resultTime <= perfectWindow / 1.8)
            {
                result = ParryResult.Perfect;
            }
            else if (resultTime <= beatMS / 2)
            {
                result = ParryResult.Late;
            }
        }

        parrying = false;
        attacked = false;

    }

    void UpdateText()
    {
        switch(result)
        {
            case(ParryResult.Early):
                text.text = "Parry - Early";
                break;
            case(ParryResult.Perfect):
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
}
