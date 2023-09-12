using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimeline : MonoBehaviour
{

    
    public SaveFileDropdown saveFileDropdown;
    public SongSave saveFile;

    public List<BlockData> eventTimeline = new List<BlockData>();

    int index;


    private void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;
        saveFile = GetComponent<SongSave>();

    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    public void StartGame()
    {
        string saveName = saveFileDropdown.GetSelectedSave();
        if(saveName != null && saveName != string.Empty)
        {
            eventTimeline = saveFile.LoadSave(saveName);
            BeatBroadcast.instance.PlayMusic();
            return;
        }
        Debug.LogError("Couldn't start game - save name was null or empty");

    }

    private void Beat(int bar, int beat)
    {
        if (saveFile == null)
        {
            Debug.LogError("No save file in BeatTimeline");
            return;
        }
        
        DoBeat(index);
        index++;

        if (index >= saveFile.SongLength)
        {
            index = 0;
        }
    }

    //TODO: Blockdata size is hardcoded to 5, figure out where this and songsave get that number from
    void DoBeat(int index)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (eventTimeline[index].events[i] != null)
            {
                eventTimeline[index].events[i].Fire();
            }
        }
    }

}
