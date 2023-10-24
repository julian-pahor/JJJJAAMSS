using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Boss : MonoBehaviour
{
   
    public Animator animator;
    public VisualEffect slash;
    public VisualEffect shockwave;
    void Update()
    {

        Vector3 direction = Wobbit.instance.player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
    }

    public void Struck()
    {
        animator.Play("damage", 0, 0f);
        shockwave.Play();
        slash.Play();
    }
}
