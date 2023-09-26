using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeFormOrbitalMove : MonoBehaviour
{

    public Transform origin;
    //movement
    [Header("Movement")]
    public float baseSpeed;
    public float dashSpeed;
    public float minDistance;
    public float maxDistance;
    public float maxDash;





    float currentDistance;
    float angle;
    
    public float CurrentAngle { get { return angle; } }

    //stats
    [Header("Stats")]
    public int maxHP;
    public int currentHP;
    public float maxShield;
    public float dashInvulnerability;
    public float hitInvulnerability;
    public float dashCooldown;


    //effects
    [Header("Effects")]
    public Color baseColour;
    public GameObject parrySphere;
    public ParticleSystem shieldFx;
    public AnimationCurve shieldPop;

    public ParticleSystem slashFx;
    public GameObject slashTransform;

    //walk puff effect
    public ParticleSystem walkPuff;
    public float puffInterval;
    float puffTimer;

    //dash effects
    public ParticleSystem dashRecover;
    public ParticleSystem dashTrail;

    public System.Action onTakeDamage;

   
    enum State { Walk,Dash,Parry,Dead}
    State state;

    //movement
    Rigidbody rb;
    float directionX;
    float directionY;  
    public Vector2 Movement { get { return new Vector2(directionX,directionY); } }
    Vector2 deltaMove;
    
    //timers
    float dashCd;
    float dashTime;
    float invulnerabilityTime;

    //flags
    bool canDash;

    Vector3 gixmo;
  
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        currentDistance = maxDistance;
        //StartCoroutine(SpeedCheck());
    }


    private void Update()
    {   
        //decrease inv timer        
        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
            parrySphere.transform.localScale = new Vector3(3f, 3f, 3f) * shieldPop.Evaluate(1 - (invulnerabilityTime / dashInvulnerability));
        }
        else
            parrySphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);


        switch (state)
        {
            case State.Walk:

                deltaMove = new Vector2(directionX, directionY);
                directionX = Input.GetAxisRaw("Horizontal");
                directionY = Input.GetAxisRaw("Vertical");
               
                dashCd -= Time.deltaTime;
                if(dashCd <= 0 && !canDash)
                {
                    canDash = true;
                    dashRecover.Play(true);
                }

                if (Input.GetKeyDown(KeyCode.Space) && dashCd <= 0)
                {
                    dashTrail.Play();
                    canDash = false;
                    dashCd = dashCooldown;
                    dashTime = maxDash;
                    invulnerabilityTime = dashInvulnerability;
                    state = State.Dash;
                }

                //walk puff
                if (Movement.magnitude > 0)
                {
                    puffTimer -= Time.deltaTime;
                    if (puffTimer < 0)
                    {
                        walkPuff.Play();
                        puffTimer = Random.Range(0, puffInterval);
                    }
                }

                break;
            case State.Dash:

                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    state = State.Walk;
                    dashTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
                   
                

                break;
            case State.Parry:

                break;
            case State.Dead:

                break;
        }

        //clamp position inside circle
        float distance = Vector3.Distance(transform.position, origin.position);
        if (distance <= minDistance && -directionY < 0) directionY = 0;
        if (distance >= maxDistance && -directionY > 0) directionY = 0;

        
    

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Walk:
            case State.Dash:
                DoMovement();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (state == State.Dead)
            return;

        if (invulnerabilityTime > 0)
        {
            shieldFx.Play();
            return;
        }
        else
        {
            invulnerabilityTime = hitInvulnerability;
            currentHP -= 1;

            if (currentHP <= 0)
            {
                state = State.Dead;
                Wobbit.instance.EndGame();
            }
               

            if (onTakeDamage != null)
            {
                onTakeDamage();
            }
        }
    }

    void DoMovement()
    {
        float currentSpeed = state == State.Dash ? dashSpeed : baseSpeed;

        //move us closer to origin based on speed
        currentDistance += -directionY * currentSpeed * Time.fixedDeltaTime;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        //move us around circle based on speed
        float arcSpeed = (currentSpeed / currentDistance) * Mathf.Rad2Deg * Time.fixedDeltaTime;
        angle += -directionX * arcSpeed;

        if (angle < 0f) angle = 360f;
        if (angle > 360f) angle = 0f;

        Vector3 moveTo = Utilities.PointWithPolarOffset(origin.position, currentDistance, angle);

        //prevent slide + reset position
        if (Movement.magnitude <= 0 || Movement != deltaMove)
        {
            moveTo = rb.position;
            Vector3 currentDirection = transform.position - origin.position;

            float currentAngle = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;
            if (currentAngle < 0) { currentAngle = 360 + currentAngle; } //do not question me
            angle = currentAngle;

            float calculatedDistance = Vector3.Distance(transform.position, origin.position);
            currentDistance = calculatedDistance;

        }

        Vector3 direction = moveTo - rb.position;

        if (direction.magnitude > .1f)
        {
            if (moveTo - transform.position != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveTo - transform.position);
                rb.rotation = targetRotation;
            }
            Vector3 adjustedMove = rb.position + (direction.normalized * currentSpeed * Time.deltaTime);

            rb.MovePosition(adjustedMove);
        }


        gixmo = moveTo;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gixmo, 2);
    }
}
