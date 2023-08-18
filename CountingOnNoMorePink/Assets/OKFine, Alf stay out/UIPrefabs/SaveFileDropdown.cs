using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

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
    void LoadSavesFromFolder()
    {
        //clear any existing files
        files.Clear();
        //grab all text files in the saves folder
        string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/SongSaves" });
        //add their names to the list
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
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
