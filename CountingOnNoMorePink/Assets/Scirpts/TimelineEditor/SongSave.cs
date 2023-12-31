using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public List<BlockData> LoadSave(string path)
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

        ///---- JSON TESTING BEGINS HERE
        ///

        //try and load our text
        Utilities.GameData thisData = Utilities.LoadData(path);

        int totalbeats = 32 + 32 + 64 + 64;
        this.phraseCount = thisData.phraseCount;
        this.phraseLength = thisData.phraseLength;
        int fileIndex = 0;

        //for every beat block
        for (int i = 0; i < totalbeats; i++)
        {
            //create set of data
            BlockData b = new BlockData(blockDataSize);

            //set each slot to the attack event (comparing names in database list)
            for (int j = 0; j < blockDataSize; j++)
            {
                AttackEvent twemp = null;
                if (thisData.fileData[fileIndex].attackEventID != "null")
                {
                   twemp = (AttackEvent)ScriptableObject.CreateInstance(thisData.fileData[fileIndex].attackEventID);
                }
                //there's no way this works
                thisData.fileData[fileIndex].DeserialiseIntoObject(twemp);
                b.events[j] = twemp;
                if(twemp != null)
                {
                    twemp.firingIndex = i;
                }

                fileIndex++;
            }
            blockDatas.Add(b);
        }

        return blockDatas;

        ///---- JSON TESTING ENDS HERE

    }

    public void SaveSong(List<Phrase> songData, string saveName)
    {

        ///----JSON TESTING BEGINS HERE
        ///

        if (songData.Count == 0)
            return;

        if (saveName == string.Empty)
        {
            Debug.LogWarning("save name cannot be blank");
            return;
        }

        Utilities.GameData gameData = new Utilities.GameData();
        gameData.phraseCount = songData.Count;

        //gameData.phraseLength = songData[0].phraseLength;
        gameData.phraseLength = 16;

        foreach (Phrase phrase in songData)
        {
            //each phrase has 16 blocks -- NOT ANYMORE
            foreach (BlockData blockData in phrase.phraseData)
            {
                //gets the array of event slots out of blockdata
                for (int i = 0; i < blockData.events.Length; i++)
                {
                    AttackEventData reference = new AttackEventData();

                    reference.SerialiseAsData(blockData.events[i]); // == null ? "null" : blockData.events[i].name;

                    gameData.fileData.Add(reference);


                }
            }
        }

        //GameData should be filled appropraitely at this point
        Utilities.SaveData(gameData, saveName);

        ///---JSON TESTING ENDS HERE

    }

    
    //public void TranslateExistingSaves(List<Phrase> songData, string saveName)
    //{
    //    ///----JSON TESTING BEGINS HERE
    //    ///

    //    if (songData.Count == 0)
    //        return;

    //    if (saveName == string.Empty)
    //    {
    //        Debug.LogWarning("save name cannot be blank");
    //        return;
    //    }

    //    Utilities.GameData gameData = new Utilities.GameData();
    //    gameData.phraseCount = songData.Count;
    //    gameData.phraseLength = songData[0].phraseLength;

    //    int phraseIndex = 0;

    //    foreach (Phrase phrase in songData)
    //    {
    //        phraseIndex++;

    //        //each phrase has 16 blocks -- NOT ANYMORE
    //        foreach (BlockData blockData in phrase.phraseData)
    //        {
    //            //gets the array of event slots out of blockdata
    //            for (int i = 0; i < blockData.events.Length; i++)
    //            {
    //                AttackEventData reference = new AttackEventData();

    //                reference.SerialiseAsData(blockData.events[i]); // == null ? "null" : blockData.events[i].name;

    //                gameData.fileData.Add(reference);


    //            }
    //        }

    //        switch (phraseIndex)
    //        {
    //            case (1):
    //                for (int i = 0; i < 16 ; i++)
    //                {
    //                    for(int y = 0; y < 5; y++)
    //                    {
    //                        gameData.fileData.Add(new AttackEventData());
    //                    }
                        
    //                }
    //                break;
    //            case (2):
    //                for (int i = 0; i < 16; i++)
    //                {
    //                    for (int y = 0; y < 5; y++)
    //                    {
    //                        gameData.fileData.Add(new AttackEventData());
    //                    }
    //                }
    //                break;
    //            case (3):
    //                for (int i = 0; i < 48; i++)
    //                {
    //                    for (int y = 0; y < 5; y++)
    //                    {
    //                        gameData.fileData.Add(new AttackEventData());
    //                    }
    //                }
    //                break;
    //            case (4):
    //                for (int i = 0; i < 48; i++)
    //                {
    //                    for (int y = 0; y < 5; y++)
    //                    {
    //                        gameData.fileData.Add(new AttackEventData());
    //                    }
    //                }
    //                break;
    //        }

    //    }

    //    //GameData should be filled appropraitely at this point
    //    Utilities.SaveData(gameData, saveName);

    //}

    public List<Phrase> GetSavedPhrases()
    {
        
        //return null if we haven't loaded any data
        if (blockDatas.Count == 0) return null;

        int index = 0; //current place in blockdata

        List<Phrase> phrases = new List<Phrase>();

        for (int i = 0; i < phraseCount; i++)
        {
            int thisLength;
            thisLength = i <= 1 ? phraseLength * 2 : phraseLength * 4;

            if(SongLength <= 64)
            {
                thisLength = phraseLength;
            }

            Phrase phrase = new Phrase(thisLength);
            for(int k = 0; k < thisLength; k++)
            {
                phrase.phraseData.Add(blockDatas[index]);
                index++;
            }
            phrases.Add(phrase);
        }
      
        return phrases;
    }

    

   
    //AttackEvent GetAttackEvent(AttackEventData data)
    //{
    //    //early exit
    //    if(name == "null")
    //        return null;

        
            
    //        data.DeserialiseIntoObject(data);

    //    foreach(AttackEvent ae in attackEventsDatabase)
    //    {
    //        if(ae.name == name)
    //            return ae;
    //    }
    //    return null;
    //}
    

}