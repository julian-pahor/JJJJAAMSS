using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelloFishingScript : MonoBehaviour
{
    //End my suffering

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(Random.Range(0,10) <= 1)
            {
                Debug.Log("Fish caught!");
            }
            else
            {
                Debug.Log("It got away...");
            }
        }
    }
}
