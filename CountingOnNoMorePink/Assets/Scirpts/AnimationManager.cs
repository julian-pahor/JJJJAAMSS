using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    private Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_anim.SetBool("Attack", false);

        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    FireAttackAnim();
        //}
    }

    public void FireAttackAnim()
    {
        int i = Random.Range(0, 5);
        m_anim.SetInteger("AttackChoice", i);
        m_anim.SetBool("Attack", true);
    }
}
