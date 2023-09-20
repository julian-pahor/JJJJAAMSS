using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimeline : MonoBehaviour
{    
    public SaveFileDropdown saveFileDropdown;
    public SongSave saveFile;

    public List<BlockData> eventTimeline = new List<BlockData>();

    int index;
    int lastBar;

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

        if (bar == 13 || bar == 14 || bar == 23 || bar == 24)
        {
            return;
        }

        if (bar < lastBar)
        {
            //Means we have looped back 
            index -= (((lastBar - bar) + 1) * 4);
            Wobbit.instance.lifeAmount = 1;
            Wobbit.instance.HealPlayer();

            if(index < 0)
            {
                index = 0;
            }
        }

        if(bar >= 5)
        {
            DoBeat(index);
            index++;
        }

        if (index >= saveFile.SongLength)
        {
            index = 0;
        }

        lastBar = bar;
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
