using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : ScriptableObject
{

    public string displayName;

    //not in use?
    public int arm;
    public int firingIndex;

    public virtual void Arm(int beatIndex)
    {
        //Debug.Log("Virtual arm attack event. If you are seeing this you have done a wrong");
    }
    public virtual void Fire()
    {
        
        //Debug.Log("Virtual attack event. If you are seeing this you have done a wrong");
    }

    public virtual void FirePreview()
    {
        Fire();
    }

    public virtual void HookUp(EventEditor ee)
    {
        //Debug.Log("Call for dynamically hooked up UI. You should not be seeing this.");
    }

}
