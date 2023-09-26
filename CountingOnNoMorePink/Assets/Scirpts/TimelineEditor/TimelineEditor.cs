using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TimelineEditor : MonoBehaviour
{
    public List<BeatBlokk> beatTimeLine = new List<BeatBlokk>();

    public BeatBlokk chunkFab;

    public Transform chunkContent;

    public TMP_Dropdown phraseSelector;


    public EventEditor eventEditor;

    public TMP_InputField saveFileNameField;
    public SongSave saveData;
    public SaveFileDropdown saveFileDropdown;
    //public GameObject previewPopUp;



    public string mainScene;

    public int phraseLength = 16;

    //list of phrases (filling in for songdata)
    public List<Phrase> phrases = new List<Phrase>();

    private int currentPhrase;
 
    void Start()
    {
        //previewPopUp.SetActive(false);
        DOTween.SetTweensCapacity(20000, 20);
        //generate 4 phrases and add to list (doing this manually for now)
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));
        phrases.Add(new Phrase(phraseLength));

        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;
        BeatBroadcast.instance.PlayPreview();

        GenerateTimelineUI();

    }


    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    public void Beat(int m, int b)
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void UpdateAllSlots()
    {
        foreach (BeatBlokk b in beatTimeLine)
        {
            b.Updoot();
        }
    }

    public void SelectEvent(AttackEvent attackEvent)
    {
        eventEditor.SelectNewObject(attackEvent);
        UpdateAllSlots();
    }

    //creates the beat blocks for each phrase in our phrase list
    void GenerateTimelineUI()
    {
        //clear out any existing blocks
        foreach (BeatBlokk b in beatTimeLine)
        {
            Destroy(b.gameObject);
        }
        beatTimeLine.Clear();

        currentPhrase = phraseSelector.value;

        //generate timeline bars (beat chunks)
        //generate a beatblock for each segment of the phrase
        for (int i = 0; i < phraseLength; i++)
        {
            BeatBlokk b = Instantiate(chunkFab, chunkContent);

            b.imig.color = b.baseColour;

            if (i == 0 || i % 4 == 0)
            {
                b.imig.color = b.beatColour;
            }

            b.Initialise(this);

            beatTimeLine.Add(b);
        }

        //save each phrase to generate initial data
        foreach (Phrase p in phrases)
        {
            p.Save(beatTimeLine);
        }
    }

    public void ChangePhrase()
    {
    
        phrases[currentPhrase].Save(beatTimeLine);

        currentPhrase = phraseSelector.value;

        phrases[currentPhrase].LoadPhraseData(beatTimeLine);

        foreach (BeatBlokk b in beatTimeLine)
        {
            StartCoroutine(b.Couroot());
        }
    }

    #region save/load

    //accesses the savesong component to create a save file
    public void TrySave()
    {
        Debug.Log("Saving...");
        phrases[currentPhrase].Save(beatTimeLine);
        saveData.SaveSong(phrases, saveFileNameField.text);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        //saveFileDropdown.LoadSavesFromFolder(); //refresh dropdown
        saveFileDropdown.LoadSavesFromPersistent();
        Debug.Log("Saved");

    }

    public void TryLoad()
    {
        if (saveFileDropdown.GetSelectedSave() == null)
            return;
        Debug.Log("Loading...");

        //reset selector
        phraseSelector.value = 0;
        currentPhrase = phraseSelector.value;

        //load save
        saveData.LoadSave(saveFileDropdown.GetSelectedSave());

        //clear our phrases and regenerate ui
        phrases.Clear();
        GenerateTimelineUI();

        //get stored phrases from data
        phrases = saveData.GetSavedPhrases();

        //load into UI view and apply to timeline
        saveFileNameField.text = saveFileDropdown.GetSelectedSave();
        phrases[currentPhrase].LoadPhraseData(beatTimeLine);
        foreach (BeatBlokk b in beatTimeLine)
        {
            //b.Updoot(true);
            StartCoroutine(b.Couroot());
        }
        Debug.Log("Load complete");
    }
    #endregion

}

//TODO: script is getting hefty, maybe break these classes into their own files

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
            bb.slots[i].LoadSlotEvent(events[i]);
        }
    }
}