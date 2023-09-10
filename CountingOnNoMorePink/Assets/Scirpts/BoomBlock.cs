using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomBlock : MonoBehaviour
{
    public float formTimeTotal;
    public float radius;
    public Vector3 startScale;
    public Vector3 endScale;
    float lerp;

    public GameObject particles;
    public GameObject trail;

    Vector3 start;
    Vector3 end;

    Collider col;
    Bullet bull;

    bool doesDrop;

    void Start()
    {
        col = GetComponent<Collider>();
      
     
        col.enabled = false;
    }

    public void Initialise(float formtime)
    {   bull = GetComponent<Bullet>();
        formTimeTotal = formtime;
        bull.maxLife = formtime;
    }
    public void Initialise(float formtime, Vector3 start, Vector3 target)
    {
        bull = GetComponent<Bullet>();
        formTimeTotal = formtime;
        bull.maxLife = formtime;
        this.start = start;
        this.end = target;
        doesDrop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp / formTimeTotal >= 1f) return;

        lerp += Time.deltaTime;
        if(doesDrop)
        {
            transform.position = Vector3.Lerp(start,end,lerp/formTimeTotal);
        }
        if(lerp/formTimeTotal >= 0.9f)
        {
            col.enabled = true;
            Instantiate(particles, transform.position, Quaternion.identity);
            transform.DetachChildren();
        }
        transform.localScale = Vector3.Lerp(startScale, endScale, lerp/formTimeTotal);

        //r.material.color = Color.Lerp(Color.white, Color.yellow, lerp / formTimeTotal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius * (1 - (lerp / formTimeTotal)));

    }
}
