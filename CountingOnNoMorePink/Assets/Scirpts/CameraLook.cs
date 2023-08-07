using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public float distance;
    public Transform focus;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 dir = transform.position - focus.position;
        transform.position = player.transform.position + (dir.normalized * distance) + new Vector3(0, offset, 0);


        transform.LookAt(focus);
    }
}
