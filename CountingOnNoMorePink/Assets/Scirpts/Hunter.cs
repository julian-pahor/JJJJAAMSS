using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{

    public Transform playerPos;
    public GameObject fab;
 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            Instantiate(fab, playerPos.position, Quaternion.identity);
        }
    }
}
