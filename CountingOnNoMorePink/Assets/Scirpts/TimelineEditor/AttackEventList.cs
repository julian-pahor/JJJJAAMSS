using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackEventList : MonoBehaviour
{
    public AttackEventHolder holderPreFab;

    public Transform contentView;

    public List<AttackEventHolder> list = new List<AttackEventHolder>();

    // Start is called before the first frame update
    void Start()
    {
        GetAllAttacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllAttacks()
    {
        list.Clear();

        List<AttackEvent> attackEvents = new List<AttackEvent>();
        attackEvents = Resources.LoadAll<AttackEvent>("AttackEvents").ToList();

        foreach(AttackEvent ae in attackEvents)
        {
            AttackEventHolder aeh = Instantiate(holderPreFab, contentView);

            aeh.SetEvent(ae);

            list.Add(aeh);
        }

    }
}
