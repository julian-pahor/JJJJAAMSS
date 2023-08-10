using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreeFormOrbitalMove : MonoBehaviour
{


    public Transform origin;
    public float baseSpeed;
    public float dashSpeed;
    public float minDistance;
    public float maxDistance;
    public float maxDash;
    public float maxShield;
    public Color baseColour;

    public GameObject parrySphere;

    Rigidbody rb;
    float directionX;
    float directionY;
    float speed;

    float hitTime;
    float dashTime;
    float shieldTime;


    bool isDash;
    bool isParry;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        HitFlash();

        //parry
        isParry = Input.GetMouseButton(1);
       

        if(isParry)
            shieldTime += Time.deltaTime;
        else
            shieldTime -= Time.deltaTime;

        shieldTime = Mathf.Clamp(shieldTime,0,maxShield);
    
        parrySphere.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(3f, 3f, 3f), shieldTime/maxShield);
       

        //dash
        dashTime -= Time.deltaTime;
        isDash = dashTime > 0;
        speed = isDash ? dashSpeed : baseSpeed;

        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = maxDash;
        }


        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");

        //julian hates optimistation
        //he is a square
        //root
        float distance = Vector3.Distance(transform.position,origin.position);

        if(distance <= minDistance && -directionY < 0) directionY = 0;
        if(distance >= maxDistance && -directionY > 0) directionY = 0;

       


    }

    void FixedUpdate()
    {

        rb.rotation  = Quaternion.LookRotation(transform.position - origin.transform.position);

        if (isParry) return;

        Vector3 direction = (transform.forward * -directionY) + (transform.right * -directionX);
        direction = direction.normalized;
        rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
      
    }


    void HitFlash()
    {
        if (hitTime > 0)
        {
            hitTime -= Time.deltaTime;
            Color color = Color.Lerp(baseColour, Color.red, hitTime);
            GetComponent<Renderer>().material.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        hitTime = 1;
    }

}
