using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Adobe.Substance.Runtime;
using Adobe.Substance;
using DG.Tweening;

public class Boss : MonoBehaviour
{
   
    public Animator animator;
    public VisualEffect slash;
    public VisualEffect shockwave;

    private SubstanceNativeGraph damageLayer;

    public SubstanceGraphSO graphSO;

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

        //This color is green :-)
        //damageLayer.SetInputColor("paintoutputcolor", new Color(24, 255, 0, damage));
        //damageLayer.SetInputVector4("paintoutputcolor", new Vector4(24, 255, 0, damage));
        //damageLayer.SetInputFloat4(2, new Vector4(24, 255, 0, damage));


        damage -= 0.001f;
    }

    public void Struck()
    {
        animator.Play("damage", 0, 0f);
        shockwave.Play();
        slash.Play();
        //transform.DOPunchPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z),.462f,4,.2f); 
        

        damage += 0.25f;

    }
}
