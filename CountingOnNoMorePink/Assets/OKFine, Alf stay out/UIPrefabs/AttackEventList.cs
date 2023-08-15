using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AttackEvent)));

        for(int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AttackEvent attackEvent = AssetDatabase.LoadAssetAtPath<AttackEvent>(assetPath);

            AttackEventHolder aeh = Instantiate(holderPreFab, contentView);

            aeh.SetEvent(attackEvent);

            list.Add(aeh);
        }

    }
}
