using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SongScoreSaver : MonoBehaviour
{

    public void SaveSongScore(string songName, SongScoreData newData)
    {

        string directoryPath = Application.persistentDataPath + "/Data/SongScores";

        //try locate directory
        DirectoryInfo di = new DirectoryInfo(directoryPath);

        if (!di.Exists)
        {
            di.Create();
        }

        string path = Application.persistentDataPath + "/Data/SongScores/" + songName + "_highscore.json";

        SongScoreData oldData = LoadSongHighscore(songName);

        if(oldData != null)
            newData = CompareScores(oldData, newData);
        

        string jsonData = JsonUtility.ToJson(newData);
       File.WriteAllText(path, jsonData);
    }


    public SongScoreData LoadSongHighscore(string songName)
    {
        string directoryPath = Application.persistentDataPath + "/Data/SongScores/" + songName + "_highscore.json";

        if(!File.Exists(directoryPath))
            return null;
     
        string json = File.ReadAllText(directoryPath);
        SongScoreData data = new SongScoreData();
        data = JsonUtility.FromJson<SongScoreData>(json);
        return data;

    }

    SongScoreData CompareScores(SongScoreData oldData, SongScoreData newData)
    {

        SongScoreData data = new SongScoreData();

        data.bestHits = Mathf.Min(oldData.bestHits, newData.bestHits);
        data.bestTotalParries = Mathf.Max(oldData.bestTotalParries, newData.bestTotalParries);
        data.bestPerfectParries = Mathf.Max(oldData.bestPerfectParries, newData.bestPerfectParries);
        data.bestHits = Mathf.Min(oldData.bestHits, newData.bestHits);

        //total attempts rather than best number
        data.attempts = Mathf.Min(oldData.attempts,newData.attempts);

        //calculate grade here
        data.grade = "X";

        return data;

    }

    
}
