using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("CURRENTLY OBSOLETE")]
    public float distance;
    public float baseSpeed;
   
    public Transform origin;
    public int maxRing;


    public float dashTime;
    public float angle;
    public float hitTime;

    public int ring = 1;
    bool isDash;
    float lerp;
    float dashCurrent;


    Vector3 from;
    Vector3 to;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(hitTime > 0)
        {
            hitTime -= Time.deltaTime;
            Color color = Color.Lerp(Color.white, Color.red, hitTime);
            GetComponent<Renderer>().material.color = color;
        }

        float speed = baseSpeed / (distance * ring);
        if (isDash)
        {
            dashCurrent += Time.deltaTime;
            lerp = dashCurrent / dashTime;
            transform.position = Vector3.Lerp(from,to,lerp);
            if(dashCurrent >= dashTime)
                isDash = false;
            else
                return;
        }
        

        if(Input.GetKeyDown(KeyCode.W))
        {
            if(ring == 1) return;
            StartDash(ring, ring -1);
            ring--;
           

        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (ring == maxRing) return;
            StartDash(ring, ring + 1);
            ring++;
            


        }

        if (Input.GetKey(KeyCode.A))
        {
            angle += speed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.D))
        {
            angle -= speed * Time.deltaTime;

        }

        if (angle < 0f) angle = 360f;
        if (angle > 360f) angle = 0f;


        transform.position = Utilities.PointWithPolarOffset(origin.position, distance * ring, angle);



    }

    private void OnDrawGizmos()
    {
       
        //for (int i = 0; i <= maxRing; i++)
        //{
        //    Gizmos.color = i == ring ? Color.blue : Color.green;
        //    Gizmos.DrawWireSphere(origin.position, distance * i);
       
        //}
    }


    void StartDash(int fromRing, int toRing)
    {
        isDash = true;
        lerp = 0;
        dashCurrent = 0;
        from = Utilities.PointWithPolarOffset(origin.position, distance * fromRing, angle);
        to = Utilities.PointWithPolarOffset(origin.position, distance * toRing, angle);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit");
        hitTime = 1;
    }

}
