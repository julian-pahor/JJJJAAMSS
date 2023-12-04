using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Adobe.Substance.Runtime;
using Adobe.Substance;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    public VisualEffect slash;
    public VisualEffect shockwave;

    private float damage = 0;

    CameraLook cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraLook>();
    }


    void Update()
    {

        Vector3 direction = Wobbit.instance.player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));

        damage -= 0.001f;
    }

    public void Struck()
    {
        //animator.Play("damage", 0, 0f);
        AnimationManager.instance.TakeDamage();
        shockwave.Play();
        slash.Play();
        //transform.DOPunchPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z),.462f,4,.2f); 
        

        damage += 0.25f;

    }
}
