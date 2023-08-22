using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SongSave : MonoBehaviour
{
    //song is list of phrases
    //phrase is list of blockdata
    //blockdata is array of attackevents
    //dogs above what have i done


    //First iteration - colossal list of blockdata
    //Later iterations - separate into phrases I guess

    public List<BlockData> blockDatas = new List<BlockData>();
    public int SongLength { get { return blockDatas.Count; } }

    public int blockDataSize; //hardcoded garbage make this a constant somewhere or get it from the data
    int phraseCount;
    int phraseLength;

    List<AttackEvent> attackEventsDatabase = new List<AttackEvent>();

    public void LoadSave(string path)
    {
        //should be empty, but clear in case
        blockDatas.Clear();

        //grab all attack event data
        List<AttackEvent> attackEvents = new List<AttackEvent>();
        attackEvents = Resources.LoadAll<AttackEvent>("AttackEvents").ToList();
        foreach (AttackEvent attackEvent in attackEvents)
        {
            attackEventsDatabase.Add(attackEvent);
        }

        //try and load our text
        List<string> fileData = new List<string>();
        phraseCount = 0;
        phraseLength = 0;

        try
        {

            using (StreamReader sr = new StreamReader(path))
            {
                //read first two lines for count and length
                int.TryParse(sr.ReadLine(), out phraseCount);
                int.TryParse(sr.ReadLine(), out phraseLength);

                //remaining data is attackevent ids
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    fileData.Add(line);
                }
            }
        }
        catch 
        {
            // will see this 90% of the time
            Debug.Log("file bad");
            return;
        }

        int totalbeats = phraseCount * phraseLength;
        int fileIndex = 0; //current position in the file

        //now setup blockdata list

        //for every beat block
        for(int i = 0; i < totalbeats; i++)
        {
            //create set of data
            BlockData b = new BlockData(blockDataSize);
            
            //set each slot to the attack event (comparing names in database list)
            for(int j = 0; j < blockDataSize; j++)
            {
                b.events[j] = GetAttackEvent(fileData[fileIndex]);
                fileIndex++;
            }
            blockDatas.Add(b);
        }

    }

    public List<Phrase> GetSavedPhrases()
    {
        
        //return null if we haven't loaded any data
        if (blockDatas.Count == 0) return null;

        int index = 0; //current place in blockdata

        List<Phrase> phrases = new List<Phrase>();

        for(int i = 0; i < phraseCount; i++)
        {
            Phrase phrase = new Phrase(phraseLength);
            for(int k = 0; k < phraseLength; k++)
            {
                phrase.phraseData.Add(blockDatas[index]);
                index++;
            }
            phrases.Add(phrase);
        }
      
        return phrases;
    }

    //TODO: this is a beattimeline thing, move it there
    public void DoBeat(int index)
    {
        for (int i = 0; i < blockDataSize; ++i)
        {
            if (blockDatas[index].events[i] != null)
            {
                blockDatas[index].events[i].Fire();
            }
        }
    }

    //TODO: change this so it's using guid and not name
    AttackEvent GetAttackEvent(string name)
    {
        //early exit
        if(name == "null")
            return null;

        foreach(AttackEvent ae in attackEventsDatabase)
        {
            if(ae.name == name)
                return ae;
        }
        return null;
    }
    //NOW LOADS FROM TEXT FILE

    //public void SaveSong(List<Phrase> songData)
    //{
    //    //wipe old save
    //    blockDatas.Clear();

    //    foreach (Phrase phrase in songData)
    //    {
    //        //each phrase has 16 blocks
    //        foreach (BlockData blockData in phrase.phraseData)
    //        {



    //            BlockData b = new BlockData(blockDataSize);

    //            for (int i = 0; i < blockDataSize; i++)
    //            {

    //                b.events[i] = blockData.events[i];
    //            }
    //            blockDatas.Add(b);
    //        }
    //    }

    //    EditorUtility.SetDirty(this);

    //}

    //oh lord whty

}