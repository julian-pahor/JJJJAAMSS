using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongScoreData
{
    public SongScoreData()
    {
        grade = "X";
    }

    public string songID;
    public int bestHits;
    public int bestTotalParries;
    public int bestPerfectParries;
    public int bestMissedParries;

    public int attempts;

    public string grade;
}
