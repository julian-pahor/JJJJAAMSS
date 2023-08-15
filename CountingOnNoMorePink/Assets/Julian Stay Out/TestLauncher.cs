using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLauncher : MonoBehaviour
{
    public Bullit bellet;
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Bullit bullit = Instantiate(bellet,transform.position,Quaternion.identity);
            bullit.Initialise(transform.forward);
        }
    }
}
