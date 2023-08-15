using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimelineEditor : MonoBehaviour
{
    public List<BeatBlokk> beatTimeLine = new List<BeatBlokk>();

    public BeatBlokk chunkFab;

    public Transform chunkContent;

    public TMP_Dropdown phraseSelector;

    public int phraseLength = 16;
    

    //public int[] phraseLength = new int[] {16,16,16,16};

    private List<List<BeatBlokk>> songData = new List<List<BeatBlokk>>();

    //list of phrases (filling in for songdata)
    public List<Phrase> phrases = new List<Phrase>();
    private int currentPhrase;
    // Start is called before the first frame update
    void Start()
    {
        //generate 4 phrases and add to list (doing this manually for now)
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));

        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;

        beatTimeLine.Clear();

        currentPhrase = phraseSelector.value;


        //generate timeline bars (beat chunks)
        //generate a beatblock for each segment of the phrase
        for (int i = 0; i < phraseLength; i++)
        {
            BeatBlokk b = Instantiate(chunkFab, chunkContent);

            if (i == 0 || i % 4 == 0)
            {
                b.GetComponent<Image>().color = Color.green;
            }

            beatTimeLine.Add(b);
        }

        //save each phrase to generate initial data
        foreach(Phrase p in phrases)
        {
            p.Save(beatTimeLine);
        }

    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    public void Beat(int m, int b)
    {

    }

    public void ChangePhrase()
    {
    
        phrases[currentPhrase].Save(beatTimeLine);

        currentPhrase = phraseSelector.value;

        phrases[currentPhrase].LoadPhraseData(beatTimeLine);

        foreach (BeatBlokk b in beatTimeLine)
        {
            b.Updoot();
        }
    }

}


//Data for a phrase(assumes these will all be the same length)

[System.Serializable]
public class Phrase
{
    public Phrase(int length)
    {
        phraseLength = length;
    }

    public int phraseLength;
    public List<BlockData> phraseData = new List<BlockData>();

    //phrase contains a list of data per block in the timeline
    //loading a new phrase replaces the current timeline data with the data from the selected phrase

    public void Save(List<BeatBlokk> songData)
    {
        //overwrite old data
        phraseData.Clear();

        //collect the data from each beatblock and save it as blockdata
        foreach(BeatBlokk bb in songData)
        {
            int count = bb.slots.Count;
            BlockData blockData = new BlockData(count);
            blockData.SaveBlockData(bb);

            phraseData.Add(blockData);

        }
    }

    public void LoadPhraseData(List<BeatBlokk> blocks)
    {
        for(int i = 0; i < phraseData.Count; i++)
        {
            phraseData[i].LoadBlockData(blocks[i]);
        }
    }
}

//blockdata is an extended array containing the data for each slot in the block
//TODO: this is highly dependent on blocks having a fixed number of slots - revisit and fix this

[System.Serializable]
public class BlockData
{

    public AttackEvent[] events;


    public BlockData(int eventsCount)
    {
        events = new AttackEvent[eventsCount];
    }
    public void SaveBlockData(BeatBlokk bb)
    {
        for(int i = 0; i < events.Length; i++)
        {
            events[i] = bb.slots[i].GetSlotEvent();
        }
    }

    public void LoadBlockData(BeatBlokk bb)
    {
        //lengths might not match so WATCH OUT
        for (int i = 0; i < events.Length; i++)
        {
            bb.slots[i].SetSlotEvent(events[i]);
        }
      
    }

}