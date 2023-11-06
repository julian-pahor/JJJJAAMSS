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

}
