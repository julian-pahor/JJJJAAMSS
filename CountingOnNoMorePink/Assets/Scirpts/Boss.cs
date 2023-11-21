using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Adobe.Substance.Runtime;
using Adobe.Substance;

public class Boss : MonoBehaviour
{
   
    public Animator animator;
    public VisualEffect slash;
    public VisualEffect shockwave;

    CameraLook cam;

    

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraLook>();
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += OnBeatAnimPlay;
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= OnBeatAnimPlay;
    }

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
        cam.StartShake();
    }

    public void OnBeatAnimPlay(int m, int b, string marker)
    {
        if(b == 1)
        {
            animator.Play("idle");
        }
    }
}
