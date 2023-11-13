using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/PersistentData")]
public class PersistentData : ScriptableObject
{
    public int songIndex;

    public int currentSongHits;

    public int currentSongRestarts;

    public int currentSongTotalParrys;

    public int currentSongPerfectParrys;

    public int currentSongMissedParrys;


    public SongScoreData GetFinalScore()
    {
        SongScoreData scoreData = new SongScoreData();

        scoreData.bestHits = currentSongHits;
        scoreData.attempts = currentSongRestarts;
        scoreData.bestTotalParries = currentSongTotalParrys;
        scoreData.bestPerfectParries = currentSongPerfectParrys;
        scoreData.bestMissedParries = currentSongMissedParrys;

        return scoreData;

    }

}
