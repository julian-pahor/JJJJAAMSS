
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PrefabEventList : MonoBehaviour
{
    public AttackEventHolder holderPreFab;

    public Transform contentView;

    public List<AttackEventHolder> holders = new List<AttackEventHolder>();

    // Start is called before the first frame update
    void Start()
    {
        LoadAll();
    }

    public void AddPrefabEvent(AttackEvent attackEvent)
    {
        AttackEventHolder aeh = Instantiate(holderPreFab, contentView);
        aeh.SetEvent(attackEvent);
        holders.Add(aeh);
    }

    public void SaveAll()
    {
        PrefabData data = new PrefabData();
        data.InitialiseFromList(GetHolderEvents());

        string path = Application.persistentDataPath + "/Data/PrefabData.json";

        System.IO.FileInfo file = new System.IO.FileInfo(path);
        file.Directory.Create(); // If the directory already exists, this method does nothing.


        string jsonData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(file.FullName, jsonData);

        //reload all data
        LoadAll();
    }


    List<AttackEvent> GetPrefabSaveData()
    {
        List<AttackEvent> list = new List<AttackEvent>();

        string path = Application.persistentDataPath + "/Data/PrefabData.json";
        //find the path
        if (File.Exists(path))
        {
            //get list of a-events from json file
            string json = File.ReadAllText(path);
            PrefabData data = JsonUtility.FromJson<PrefabData>(json);

            //for each event we find, create a new attack event
            foreach(AttackEventData attackEventData in data.prefabEventData)
            {
                AttackEvent tempAE = null;
                if (attackEventData.attackEventID != "null")
                {
                    tempAE = (AttackEvent)ScriptableObject.CreateInstance(attackEventData.attackEventID);
                    //load data into event
                    attackEventData.DeserialiseIntoObject(tempAE);
                    
                }
                //add event to list
                list.Add(tempAE);
            }

        }
        else
            Debug.Log("No data found for prefabs");

        //pass back list
        return list;
    }


    List<AttackEvent> GetHolderEvents()
    {
        List<AttackEvent> eventList = new List<AttackEvent>();
        foreach(AttackEventHolder holder in holders)
        {
            eventList.Add(holder.attackEvent);
        }
        return eventList;
    }

    public void LoadAll()
    {
        //wipe holders
        foreach(AttackEventHolder holder in holders)
        {
            Destroy(holder.gameObject);
        }

        holders.Clear();

        List<AttackEvent> attackEvents = GetPrefabSaveData();

        foreach (AttackEvent ae in attackEvents)
        {
            AddPrefabEvent(ae);
        }
    }


    class PrefabData
    {
        public void InitialiseFromList(List<AttackEvent> list)
        {
            foreach (AttackEvent ae in list)
            {
                AttackEventData aed = new AttackEventData();
                aed.SerialiseAsData(ae);
                prefabEventData.Add(aed);
            }
        }

        public List<AttackEventData> prefabEventData = new List<AttackEventData>();
    }
}
