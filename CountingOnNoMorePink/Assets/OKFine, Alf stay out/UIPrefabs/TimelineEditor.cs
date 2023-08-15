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

    private int currentPhrase;

    public int[] phraseLength = new int[] {16,16,16,16};

    private List<List<BeatBlokk>> songData = new List<List<BeatBlokk>>();

    // Start is called before the first frame update
    void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;

        beatTimeLine.Clear();

        currentPhrase = phraseSelector.value;


        

        for(int i = 0; i < phraseLength[currentPhrase]; i++)
        {
            BeatBlokk b = Instantiate(chunkFab, chunkContent);
            
            if(i == 0 || i % 4 == 0)
            {
                b.GetComponent<Image>().color = Color.green;
            }

            beatTimeLine.Add(b);
        }

        for (int i = 0; i < phraseLength.Length; i++)
        {
            List<BeatBlokk> clone = new List<BeatBlokk>(beatTimeLine);
            
            songData.Add(clone);
        }


    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    public void ChangePhrase()
    {
        if(currentPhrase == phraseSelector.value)
        {
            return;
        }

        List<BeatBlokk> clone = new List<BeatBlokk>(beatTimeLine);

        songData[currentPhrase] = clone;

        currentPhrase = phraseSelector.value;

        foreach(BeatBlokk bb in beatTimeLine)
        {
            Destroy(bb.gameObject);
        }

        beatTimeLine.Clear();

        //read current phrase out of phraseSelector.value (0-3 -> 1-4)
    }

    public void Beat(int m, int b)
    {

    }
}
