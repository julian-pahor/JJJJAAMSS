using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    SphereCollider wavesphere;
    public Transform sphere;

    public float maxSize;
    public float propogationSpeed;
    float currentSize;

    Vector3 waveInitialScale;
    Vector3 waveMaxScale;

    private void Start()
    {
        wavesphere = GetComponent<SphereCollider>();
        waveInitialScale = Vector3.one * wavesphere.radius *2;
        waveMaxScale = Vector3.one * maxSize * 2;
    }
    void Update()
    {
        currentSize += propogationSpeed * Time.deltaTime;
        wavesphere.radius = currentSize;

        sphere.localScale = Vector3.Lerp(waveInitialScale, waveMaxScale, currentSize/maxSize);

        if(currentSize >= maxSize)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PooledObject po = other.GetComponent<PooledObject>();
  
        if(po != null)
        {
            Debug.Log(po.gameObject.name);
            po.Despawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, currentSize);
    }
}
