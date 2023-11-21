using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Windows;
using System.IO;
using DG.Tweening.Plugins;

//Helper component that checks the SongSave folder for save files and creates a dropdown list of them
//returns a string that should correspond to the name of the file we want to load
public class SaveFileDropdown : MonoBehaviour
{

    List<string> files = new List<string>();
    public TMP_Dropdown dropdown;

    public PersistentData persistentData;

    // Start is called before the first frame update
    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        //LoadSavesFromFolder();
        //LoadSavesFromPersistent();
        LoadSaves();
        SetCurrentIndex(persistentData.songIndex);
        //Debug.Log(files[persistentData.songIndex]);
    }

    //TODO: THIS WILL NOT WORK IN BUILD (apparently) - FIND A WAY THAT WILL (https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html)
    //Julian has been here and has fixed it so now it will work in build :^)
    //THIS STILL DOES NOT WORK IN BUILD JULIAN HAS BETRAYED US ALL
    public void LoadSavesFromFolder()
    {
        //clear any existing files
        files.Clear();
        dropdown.ClearOptions();

        List<TextAsset> songSaves = new List<TextAsset>();
        songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();
        foreach (TextAsset file in songSaves)
        {
            files.Add(file.name);
        }

        dropdown.AddOptions(files);

        ///TRANSPORTING DATA TO PERSISTENT PATH
        ///
        foreach (TextAsset file in songSaves)
        {
            string jsonData = file.text;
            string path = Application.persistentDataPath + "/SongSaves/" + file.name + ".json";
            System.IO.File.WriteAllText(path, jsonData);
        }
    }

    /// <summary>
    /// Ok so this is appropriately going into the persisdent data path folder
    /// and adding the name optioins correctly to the dropdown
    /// </summary>
    public void LoadSavesFromPersistent()
    {
        files.Clear();
        dropdown.ClearOptions();

        string path = Application.persistentDataPath + "/SongSaves/";

        if(Utilities.DirectoryStuff(path))
        {
            foreach(var file in System.IO.Directory.GetFiles(path))
            {
                Debug.Log(path);
                string filePath = file.Replace(path, "");
                filePath = filePath.Replace(".json", "");
                files.Add(filePath);
                //string json = System.IO.File.ReadAllText(files);
            }

            dropdown.AddOptions(files);
        }
        else
        {
            //Called if the directory has not been created yet to move all existing saves
            //from resources into the persistent data path 
            LoadSavesFromFolder();
        }
    }

    public void LoadSaves()
    {
        files.Clear();
        dropdown.ClearOptions();

        string path = Application.persistentDataPath + "/SongSaves/";

        Utilities.SaveNames saves = new Utilities.SaveNames();
        saves = Utilities.CheckGetSaves(path);

        files = saves.baseLevels;

        dropdown.AddOptions(saves.baseLevels);
    }

    public void SetCurrentIndex(int index)
    {
        if (index >= files.Count)
            return;
        dropdown.value = index;
        dropdown.RefreshShownValue();
    }

    public string GetSelectedSave()
    {
        if (files.Count == 0)
            return null;

        return files[dropdown.value];
    }

    public List<string> GetAllSaves()
    {
        return files;
    }
    
    public void StoreSongIndex()
    {
        persistentData.songIndex = dropdown.value;
    }

    public string SongNameFromIndex(int index)
    {
        return files[index];
    }

    //#if UNITY_EDITOR
    //    [ContextMenu("PortSaves")]
    //    public void PortSavesToPersistent()
    //    {
    //        //TODO: 
    //        //Copy all saves in resources folder to persistent datapath 
    //    }
    //#endif

    //[ContextMenu("FINDPATH")]
    //public void LoadSavesFromPersistent()
    //{
    //    files.Clear();
    //    dropdown.ClearOptions();


    //    Debug.Log(Application.persistentDataPath);
    //}





    //[ContextMenu("TRANSLATE (BECAREFUL) !AHHH!")]
    //public void TranslateCurrentSaves()
    //{
    //    List<TextAsset> songSaves = new List<TextAsset>();
    //    songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();

    //    List<Utilities.GameData> translatedSaves = new List<Utilities.GameData>();

    //    foreach (TextAsset file in songSaves)
    //    {
    //        translatedSaves.Add(Utilities.TranslateData("Assets/Resources/SongSaves/" + file.name + ".txt"));
    //    }

    //    for (int i = 0; i < translatedSaves.Count; i++)
    //    {
    //        Utilities.SaveData(translatedSaves[i], songSaves[i].name);
    //    }
    //}

    
}
