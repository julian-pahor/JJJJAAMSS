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
    public float hpRecoveryTime;
    private float recoverTimer;
    public float dashCooldown;
    public float parryCooldown;

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
    public System.Action onHealthChanged;


    //animation
    public Animator animator;
   
    enum State { Walk,Dash,Parry,Dead}
    State state;

    //movement
    Rigidbody rb;
    float directionX;
    float directionY;  
    public Vector2 Movement { get { return new Vector2(directionX,directionY); } }
    Vector2 deltaMove;

    //parry
    Parry parryHandler;
    
    //timers
    float dashCd;
    float dashTime;
    float invulnerabilityTime;
    float parryCd;

    //flags
    bool canDash;

    Vector3 gixmo;
  
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        currentDistance = maxDistance;
        recoverTimer = hpRecoveryTime;
        parryHandler = GetComponent<Parry>();
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

        //Timed Heal on Player
        if (currentHP < maxHP && invulnerabilityTime <= 0)
        {
            recoverTimer -= Time.deltaTime;
            if (recoverTimer < 0)
            {
                recoverTimer = hpRecoveryTime;
                currentHP += 1;
                onHealthChanged?.Invoke();
            }
        }

        switch (state)
        {
            case State.Walk:

               
               
                directionX = Input.GetAxisRaw("Horizontal");
                directionY = Input.GetAxisRaw("Vertical");

                Vector2 movement = new Vector2(directionX,directionY);

                animator.SetBool("Moving", movement != Vector2.zero);
              

                //dash recovery
                dashCd -= Time.deltaTime;
                if(dashCd <= 0 && !canDash)
                {
                    canDash = true;
                    dashRecover.Play(true);
                }

                //parry
                if(parryCd > 0f)
                    parryCd -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Parry"))
                {
                    if(parryCd <= 0)
                    {
                        parryHandler.DoParry();
                        parryCd = parryCooldown;
                    }

                }

                //dash
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && dashCd <= 0)
                {
                    if (movement != Vector2.zero)
                        animator.Play("dash", 0, 0f);
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

    void TakeDamage()
    {
        invulnerabilityTime = hitInvulnerability;
        recoverTimer = hpRecoveryTime;
        currentHP -= 1;
        animator.Play("hurt", 0, 0f);


        if (currentHP <= 0)
        {
            state = State.Dead;
            animator.Play("death", 0, 0f);
            Wobbit.instance.EndGame();
        }

        onTakeDamage?.Invoke();
        onHealthChanged?.Invoke();
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
            TakeDamage();
        }
    }

    void DoMovement()
    {
        float currentSpeed = state == State.Dash ? dashSpeed : baseSpeed;

        //normalise the input (you spud)
        Vector2 movement = new Vector2(directionX, directionY).normalized;

        //move us closer to origin based on speed
        currentDistance += -movement.y * currentSpeed * Time.fixedDeltaTime;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        //move us around circle based on speed
        float arcSpeed = (currentSpeed / currentDistance) * Mathf.Rad2Deg * Time.fixedDeltaTime;
        angle += -movement.x * arcSpeed;


        if (angle < 0f) angle = 360f;
        if (angle > 360f) angle = 0f;

        Vector3 moveTo = Utilities.PointWithPolarOffset(origin.position, currentDistance, angle);

        Vector3 lookDir = moveTo - transform.position;
        if(lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            rb.rotation = targetRotation;
        }

        rb.MovePosition(moveTo);

        
    
        

        //-------------------------------------------------------------------------------------------------------------------------------
        //Everything below is utterly useless because the solution all along has been to normalise the input which is how you would do it
        //for regular movement anyway but I tricked myself into believing it was complicated because I am addicted to suffering
        //-------------------------------------------------------------------------------------------------------------------------------

        //prevent slide + reset position
        //if (Movement.magnitude <= 0)
        //{
        //    moveTo = rb.position;
        //    RecalculatePosition();
        //}

        // Vector3 direction = moveTo - rb.position;

        //if (direction.magnitude > .1f)
        //{
        //    if (moveTo - transform.position != Vector3.zero)
        //    {
        //        Quaternion targetRotation = Quaternion.LookRotation(moveTo - transform.position);
        //        rb.rotation = targetRotation;
        //    }
        //    Vector3 adjustedMove = rb.position + (direction.normalized * currentSpeed * Time.deltaTime);
        //    rb.MovePosition(adjustedMove);


        //}

        //void RecalculatePosition()
        //{
        //    Vector3 currentDirection = transform.position - origin.position;

        //    //calculate angle from centre to player
        //    float currentAngle = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;
        //    if (currentAngle < 0) { currentAngle = 360 + currentAngle; } //do not question me
        //    angle = currentAngle;

        //    //calculated distance from centre to player
        //    float calculatedDistance = Vector3.Distance(transform.position, origin.position);
        //    currentDistance = calculatedDistance;
        //}


        gixmo = moveTo;

    }

    public bool IsAlive()
    {
        return state != State.Dead;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gixmo, 2);
    }
}
