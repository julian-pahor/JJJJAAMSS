using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{

    public GameObject hitBox;

    public float leadup;
    float currentLeadup;
    public float maxRadius;

    private void Start()
    {
        currentLeadup = leadup;
    }
    private void Update()
    {
        
        currentLeadup -= Time.deltaTime;

        if(currentLeadup <= 0)
        {
            Instantiate(hitBox,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius * (currentLeadup/leadup));
    }
}
