using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : ScriptableObject
{

    public virtual void Fire()
    {
        Debug.Log("Virtual attack event. If you are seeing this you have done a wrong");
    }

}
