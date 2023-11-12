using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialHealthBar : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public Transform canvas;
    public Transform rotTarget;

    public Gradient gradient;
    public AnimationCurve curve;

    public float hitFlashDuration;
    

    public float flashLength;
    public float healthFlashThreshold;

    Image image;
    float healthPercentage;
    float currentFlashAmount;
    float hitFlashTimer;

    Color currentColour;


    private void Start()
    {
        image = GetComponent<Image>();
        player.onTakeDamage += Hit;
    }

    private void Update()
    {
        if(player == null)
            return;

        healthPercentage = player.currentHP / player.maxHP;

        image.enabled = player.currentHP < player.maxHP;

        image.fillAmount = healthPercentage;

        DoColour();

        RotateTowardsTarget();
    }


    void Hit()
    {
        hitFlashTimer = hitFlashDuration;
    }

    void DoColour()
    {
      
        if (hitFlashTimer > 0)
            hitFlashTimer -= Time.deltaTime;

        currentColour = gradient.Evaluate(healthPercentage);


        //flash healthbar if we just took a hit, or if we're close to death
        if (hitFlashTimer > 0 || healthPercentage <= healthFlashThreshold)
        {
            currentFlashAmount = Mathf.PingPong(Time.time, flashLength);
            image.color = Color.Lerp(currentColour, Color.white, curve.Evaluate(currentFlashAmount / flashLength));
        }
        else
        {

            image.color = currentColour;
        }

    }

    void RotateTowardsTarget()
    {
        if (rotTarget == null || canvas == null)
            return;

        Vector2 ourPosition = new Vector2(canvas.transform.position.x,canvas.transform.position.z);
        Vector2 theirPosition = new Vector2(rotTarget.transform.position.x,rotTarget.transform.position.z);

        Vector2 direction = theirPosition - ourPosition;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        canvas.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, angle));

    }
}
