using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ParryReturn2 : MonoBehaviour
{

    private Vector3 target;
    public float travelTime;
    private float currentTime;
    private float currentLerp;
    public Transform hitFX;


    // Start is called before the first frame update
    void Start()
    {
        target = Wobbit.instance.bossOrigin.position + Vector3.up * 10f;
        travelTime = 0.462f * 1.5f;
        hitFX.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * 75f * Time.deltaTime);

        currentLerp = currentTime / travelTime;
        transform.position = Vector3.Lerp(transform.position, target, currentLerp);
        currentTime += Time.deltaTime;

        if(currentLerp > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
