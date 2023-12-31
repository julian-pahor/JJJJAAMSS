using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 direction;
    public float speed;
    public float maxLife;

    float lifespan;
    public bool flying;
    public bool permanent;

    
    public void Initialise(Vector3 dir)
    {
        direction = dir;
        transform.LookAt(transform.position + dir.normalized);
        lifespan = 0;
   
    }


   

    void Update()
    {
        if(!permanent)
            lifespan += Time.deltaTime;

        if(flying)transform.position += direction.normalized * speed * Time.deltaTime;

        if (lifespan > maxLife)
            GetComponent<PooledObject>().Despawn();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<BulletKillSphere>() != null)
        {
            GetComponent<PooledObject>().Despawn();
        }
    }
}
