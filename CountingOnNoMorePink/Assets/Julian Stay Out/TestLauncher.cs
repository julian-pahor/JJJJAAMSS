using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLauncher : MonoBehaviour
{
    public Bullet bellet;
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Bullet bullit = Instantiate(bellet,transform.position,Quaternion.identity);
            bullit.Initialise(transform.forward);
        }
    }
}
