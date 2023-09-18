using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeFormOrbitalMove : MonoBehaviour
{


    public Transform origin;
    //movement
    public float baseSpeed;
    public float dashSpeed;
    public float minDistance;
    public float maxDistance;
    public float maxDash;

    public float dashInvulnerability;
    public float hitInvulnerability;

    float currentDistance;
    float angle;
    public float CurrentAngle { get { return angle; } }

    //stats and effects
    public int maxHP;
    public int currentHP;
    public float maxShield;
    public Color baseColour;
    public GameObject parrySphere;
    public ParticleSystem shieldFx;
    public ParticleSystem slashFx;
  
    public GameObject slashTransform;

    public AnimationCurve shieldPop;

    public System.Action onTakeDamage;

    //walk puff effect
    public ParticleSystem walkPuff;
    public float puffInterval;
    float puffTimer;

    //movement
    Rigidbody rb;
    float directionX;
    float directionY;  
    public Vector2 Movement { get { return new Vector2(directionX,directionY); } }

    float dashTime;
    float invulnerabilityTime;
    float shieldTime;

    //flags
    bool isDash;
    bool isParry;


    Vector3 gizmoPoint;
    Vector3 gizmoPoint2;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        currentDistance = maxDistance;
        //StartCoroutine(SpeedCheck());
    }


    private void Update()
    {
        HitFlash();

        //parry
        isParry = Input.GetMouseButton(1);

        if (isParry)
            shieldTime += Time.deltaTime;
        else
            shieldTime -= Time.deltaTime;

        shieldTime = Mathf.Clamp(shieldTime, 0, maxShield);

        //dash
        dashTime -= Time.deltaTime;
        isDash = dashTime > 0;

        if (isParry)
            parrySphere.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(3f, 3f, 3f), shieldTime / maxShield);
        else if (invulnerabilityTime > 0)
        {
            parrySphere.transform.localScale = new Vector3(3f, 3f, 3f) * shieldPop.Evaluate(1 - (invulnerabilityTime/dashInvulnerability));
        }
        else
            parrySphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = maxDash;
            invulnerabilityTime = dashInvulnerability;
        }

        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");

        if (Movement.magnitude > 0)
        {
            puffTimer -= Time.deltaTime;
            if(puffTimer < 0)
            {
                walkPuff.Play();
                puffTimer = Random.Range(0,puffInterval);
            }
        }
      
        //julian hates optimistation
        //he is a square
        //root
        float distance = Vector3.Distance(transform.position, origin.position);


        //TODO: some kind of bug where dashing while moving diagonally will let you go past the max distance
        //may be a non-issue once we rework the dash
        if (distance <= minDistance && -directionY < 0) directionY = 0;
        if (distance >= maxDistance && -directionY > 0) directionY = 0;



    }

    void FixedUpdate()
    {

       // rb.rotation = Quaternion.LookRotation(transform.position - origin.transform.position);

        if (isParry) return;

        float currentSpeed = isDash ? dashSpeed : baseSpeed;

        //move us closer to origin based on speed
        currentDistance += -directionY * currentSpeed * Time.fixedDeltaTime;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        //move us around circle based on speed
        float arcSpeed = (currentSpeed / currentDistance) * Mathf.Rad2Deg * Time.fixedDeltaTime;
        angle += -directionX * arcSpeed;
        
        if (angle < 0f) angle = 360f;
        if (angle > 360f) angle = 0f;

        Vector3 moveTo = Utilities.PointWithPolarOffset(origin.position, currentDistance, angle);

        if(Movement.magnitude <= 0)
        {
            moveTo = rb.position;
        }

        //Vector3 direction = (transform.forward * -directionY) + (transform.right * -directionX);
        //direction = direction.normalized;

        // transform.LookAt(moveTo);

        gizmoPoint = moveTo;

        Vector3 direction = moveTo - rb.position;

        if (direction.magnitude > .75f)
        {
            if (moveTo - transform.position != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveTo - transform.position);
                rb.rotation = targetRotation;
            }
            Vector3 adjustedMove = rb.position + (direction.normalized * currentSpeed * Time.deltaTime);
            gizmoPoint2 = adjustedMove;
            rb.MovePosition(adjustedMove);
        }
        

    }


    void HitFlash()
    {
        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (invulnerabilityTime > 0)
        {
            shieldFx.Play();
            return;
        }
           
        if (isParry && shieldTime >= (maxShield - 0.01))
        {
            shieldFx.Play();
        }
        else
        {
            invulnerabilityTime = hitInvulnerability;
            currentHP -= 1;

            if (currentHP <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            if (onTakeDamage != null)
            {
                onTakeDamage();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gizmoPoint, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoPoint2, 1f);
    }

}
