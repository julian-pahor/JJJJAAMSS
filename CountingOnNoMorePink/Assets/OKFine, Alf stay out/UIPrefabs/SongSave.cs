using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEditor;

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

    List<AttackEvent> attackEventsDatabase = new List<AttackEvent>();

    public void LoadSave(string path)
    {
        //should be empty, but clear in case
        blockDatas.Clear();

        //grab all attack event data

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(AttackEvent)));

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AttackEvent attackEvent = AssetDatabase.LoadAssetAtPath<AttackEvent>(assetPath);
            attackEventsDatabase.Add(attackEvent);
        }

        //try and load our text
        List<string> fileData = new List<string>();
        int phraseCount = 0;
        int phraseLength = 0;

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