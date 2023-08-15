using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public float distance;
    public Transform focus;
    public float offset;

    bool isShake;
    // Start is called before the first frame update
    void Start()
    {
        player.onTakeDamage += StartShake;
    }

    // Update is called once per frame
    void Update()
    {
     
        
        Vector3 dir = transform.position - focus.position;
        transform.position = player.transform.position + (dir.normalized * distance) + new Vector3(0, offset, 0);


        transform.LookAt(focus);
    }


    void StartShake()
    {
        StartCoroutine(Shake(.2f, .5f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        isShake = true;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        isShake = false;
        //transform.position = orignalPosition;
    }
}
