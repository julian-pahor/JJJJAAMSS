using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public float distance;
    public float focusDistance;
    public Transform focus;
    public float offset;
    public float speed;

    Vector3 target;
    Vector3 lookTarget;

    // Start is called before the first frame update
    void Start()
    {
        player.onTakeDamage += StartShake;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = player.CurrentAngle;
        float adjustedSpeed = speed * Vector3.Distance(target, transform.position);

        target = Utilities.PointWithPolarOffset(player.transform.position, distance, angle) + new Vector3(0, offset, 0);

        transform.position = Vector3.MoveTowards(transform.position,target,adjustedSpeed * Time.deltaTime);
        //lt incredible mathfs
        lookTarget = Utilities.PointWithPolarOffset(focus.position, distance - (focusDistance + Vector3.Distance(player.transform.position, focus.position)), angle + 180f);

        transform.LookAt(lookTarget);
    }


    void StartShake()
    {
        StartCoroutine(Shake(.2f, .5f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;


        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        //transform.position = orignalPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lookTarget, 2f);
    }
}
