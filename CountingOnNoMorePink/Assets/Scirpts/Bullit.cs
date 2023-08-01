using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullit : MonoBehaviour
{
    Vector3 direction;
    public float speed;
    public float maxLife;

    float lifespan;
    public bool flying;
    
    public void Initialise(Vector3 dir)
    {
        direction = dir;
        lifespan = 0;
    }

    void Update()
    {
        lifespan += Time.deltaTime;
        if(flying)transform.position += direction.normalized * speed * Time.deltaTime;
        if(lifespan > maxLife)
            Destroy(gameObject);
    }
}
