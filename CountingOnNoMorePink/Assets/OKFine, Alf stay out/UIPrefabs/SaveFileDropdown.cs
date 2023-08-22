using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

//Helper component that checks the SongSave folder for save files and creates a dropdown list of them
//returns a string that should correspond to the name of the file we want to load
public class SaveFileDropdown : MonoBehaviour
{

    List<string> files = new List<string>();
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        LoadSavesFromFolder();
    }

    //TODO: THIS WILL NOT WORK IN BUILD (apparently) - FIND A WAY THAT WILL (https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html)
    //Julian has been here and has fixed it so now it will work in build :^)
    public void LoadSavesFromFolder()
    {
        //clear any existing files
        files.Clear();
        dropdown.ClearOptions();

        List<TextAsset> songSaves = new List<TextAsset>();
        songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();
        foreach(TextAsset file in songSaves)
        {
            files.Add(file.name);
        }

        dropdown.AddOptions(files);
    }

    public string GetSelectedSave()
    {
        if (files.Count == 0)
            return null;

        return files[dropdown.value];
    }
}
