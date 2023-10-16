using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimeline : MonoBehaviour
{    
    public SaveFileDropdown saveFileDropdown;
    public SongSave saveFile;
    public int lastCheckpoint = 0;

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

        //Set playback checkpoint
        //TODO: Set playback index correctly based on lastCheckpoint
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Checkpoint", lastCheckpoint);

        if (saveName != null && saveName != string.Empty)
        {
            eventTimeline = saveFile.LoadSave(saveName);
            BeatBroadcast.instance.PlayMusic();
            return;
        }

        Debug.LogError("Couldn't start game - save name was null or empty");

    }

    private void Beat(int bar, int beat, string marker)
    {
        if (saveFile == null)
        {
            Debug.LogError("No save file in BeatTimeline");
            return;
        }

        //Finish + End are one time calls used to tidy up messes 
        //End of music / Passed the level
        if(marker == "Finish")
        {
            Wobbit.instance.FinishSong();
            return;
        }

        //End of Audio Event / Last call from Beat Broadcast
        if(marker == "End")
        {
            return;
        }

        //Currently stops functionality during transitional bars
        //TODO: Move transitions to a seperate event that automatically stops and resumes timeline event
        if (bar == 13 || bar == 14 || bar == 23 || bar == 24)
        {
            return;
        }

        if(bar >= 5)
        {
            DoBeat(index);
            DoArm(index);
            index++;
        }

        //looping index of beat timeline
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

    void DoArm(int index)
    {
        for(int i = 1; i < 4; i++)
        {
            if(index + i > eventTimeline.Count - 1)
            {
                return;
            }

            for(int j = 0; j < 5; j++)
            {
                if (eventTimeline[index + i].events[j] != null)
                {
                    eventTimeline[index + i].events[j].Arm(index);
                }
            }
        }
    }

}
