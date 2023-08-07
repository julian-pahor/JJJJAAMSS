using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLife : MonoBehaviour
{
    public float maxLife;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxLife) Destroy(gameObject);
    }
}
