using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public PlayerMovement player;
    public float distance;
    public Transform focus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Utilities.PointWithPolarOffset(focus.position, distance * player.ring, player.angle);
        transform.position += new Vector3(0, 17, 0);

        transform.LookAt(focus);
    }
}
