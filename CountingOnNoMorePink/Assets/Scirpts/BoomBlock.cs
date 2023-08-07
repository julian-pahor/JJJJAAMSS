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

    Collider col;
    Bullit bull;
    Renderer r;
    void Start()
    {
        col = GetComponent<Collider>();
        r = GetComponent<Renderer>();
     
        col.enabled = false;
    }

    public void Initialise(float formtime)
    {   bull = GetComponent<Bullit>();
        formTimeTotal = formtime;
        bull.maxLife = formtime;

    }


    // Update is called once per frame
    void Update()
    {
        if (lerp / formTimeTotal >= 1f) return;

            lerp += Time.deltaTime;
        if(lerp/formTimeTotal >= 0.9f)
        {
            col.enabled = true;
            Instantiate(particles, transform.position, Quaternion.identity);
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
