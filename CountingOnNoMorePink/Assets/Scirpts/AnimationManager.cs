using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    private Animator m_anim;

    private AnimatorClipInfo[] m_CurrentClipInfo;

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

    }

    public void FireAttackAnim()
    {

        m_CurrentClipInfo = m_anim.GetCurrentAnimatorClipInfo(0);
        if (m_CurrentClipInfo[0].clip.name != "Idle1_ANI" && m_CurrentClipInfo[0].clip.name != "Idle2_ANI")
        {
            return;
        }

        int i = Random.Range(0, 2);

        m_anim.SetInteger("AttackChoice", i);

        switch (i)
        {
            case (0):
                m_anim.Play("Parry1");
                break;
            case (1):
                m_anim.Play("Parry2");
                break;

        }
    }

    public void FireDeathAnim()
    {
        m_anim.Play("Dying");
    }

    public void TakeDamage()
    {
        m_anim.Play("damage");
    }
}
