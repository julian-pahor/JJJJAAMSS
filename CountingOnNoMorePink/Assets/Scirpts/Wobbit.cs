using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbit : MonoBehaviour
{
  public static Wobbit instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("oh no another wobbit");
            return;
        }
        instance = this;
    }

    public Bullit bulletFab;
    public BoomBlock zoneFab;
    public Transform bossOrigin;
    public Transform player;

}
