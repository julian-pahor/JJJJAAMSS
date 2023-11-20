using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public static class Utilities
{

    public class SaveNames
    {
        public List<string> baseLevels;
        public List<string> customLevels;

        public SaveNames()
        {
            baseLevels = new List<string>();
            customLevels = new List<string>();
        }

    }
    /// <summary>
    /// Returns a point in a polar direction at a specified distance from our origin
    /// </summary>
    public static Vector2 PointWithPolarOffset(Vector2 origin, float distance, float angle)
    {
        Vector2 point;



        point.x = origin.x + Mathf.Sin((angle * Mathf.PI) / 180) * distance;
        point.y = origin.y + Mathf.Cos((angle * Mathf.PI) / 180) * distance;



        return point;
    }


    /// <summary>
    /// Returns a point in a polar direction at a specified distance from our origin
    /// </summary>
    public static Vector3 PointWithPolarOffset(Vector3 origin, float distance, float angle)
    {
        Vector3 point;


        point.y = origin.y;
        point.x = origin.x + Mathf.Sin((angle * Mathf.PI) / 180) * distance;
        point.z = origin.z + Mathf.Cos((angle * Mathf.PI) / 180) * distance;



        return point;
    }


    

    public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

    /// <summary>
    /// Quadratic lerp that slashes through point a
    /// </summary>
    public static Vector3 QuadraticSlice(Vector3 a, Vector3 b, Vector3 c, float t)
    {

        Vector3 Px = QuadraticLerp(a, b, c, .5f); //midpoint of arc
        Vector3 offset = Px - a;

        return QuadraticLerp(a - offset, b, c, t);

    }




    //Save and Load DataClasses And Functionality

    public class GameData
    {
        //Load it up with variables of whatever we need to save
        public int phraseCount;
        public int phraseLength;

        //public List<string> fileData;

        public List<AttackEventData> fileData;

        public GameData()
        {
            fileData = new List<AttackEventData>();
        }
    }

    //I busted this it's dead
    /*
    public static GameData TranslateData(string path)
    {
        GameData data = new GameData();

        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                //read first two lines for count and length
                int.TryParse(sr.ReadLine(), out data.phraseCount);
                int.TryParse(sr.ReadLine(), out data.phraseLength);

                //remaining data is attackevent ids
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //fileData.Add(line);
                    data.fileData.Add(line);
                }
            }

            return data;
        }
        catch
        {
            // will see this 90% of the time
            Debug.Log("file bad");
            return null;
        }
        
    }

    */
    public static void SaveData(GameData saveData, string saveName)
    {

//#if UNITY_EDITOR
//        string path = $"Assets/Resources/SongSaves/{saveName}.json";

//#else
//        string path = Application.persistentDataPath + "/SongSaves/" + saveName + ".json";
//#endif


//        string jsonData = JsonUtility.ToJson(saveData);
//        System.IO.File.WriteAllText(path, jsonData);




        //PERSISTENT DATA PATH LOADING STARTS HERE----------

        string path = Application.persistentDataPath + "/SongSaves/CustomLevels/" + saveName + ".json";
        string jsonData = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(path, jsonData);


    }

    public static GameData LoadData(string saveName)
    {
        //PERSISTENT DATA PATH LOADING STARTS HERE----------
        //Formatting string correctly to find folder in persistent data path
        string path = Application.persistentDataPath + "/SongSaves/";

        DirectoryInfo di = new DirectoryInfo(path);

        //Checks if direcotory existed on computer
        if (di.Exists)
        {
            string baseLevels = path + "BaseLevels/";
            string customLevels = path + "CustomLevels/";

            foreach (var file in System.IO.Directory.GetFiles(baseLevels))
            {
                //Formatting file names to just be the save name 
                string filePath = file.Replace(baseLevels, "");
                filePath = filePath.Replace(".json", "");
                if(filePath == saveName)
                {
                    string json = System.IO.File.ReadAllText(file);
                    GameData data = new GameData();
                    data = JsonUtility.FromJson<GameData>(json);
                    return data; 
                }
            }

            foreach (var file in System.IO.Directory.GetFiles(customLevels))
            {
                //Formatting file names to just be the save name 
                string filePath = file.Replace(customLevels, "");
                filePath = filePath.Replace(".json", "");
                if (filePath == saveName)
                {
                    string json = System.IO.File.ReadAllText(file);
                    GameData data = new GameData();
                    data = JsonUtility.FromJson<GameData>(json);
                    return data;
                }
            }
            Debug.Log("Found Directory but could not find save");
        }
         
         Debug.Log("That directory doesn't exist oops");
         return new GameData();
       
    }

    public static void DeleteData(string saveName)
    {
#if UNITY_EDITOR
        string path = $"Assets/Resources/SongSaves/{saveName}.json";

#else
        string path = Application.persistentDataPath + "/SongSaves/" + saveName + ".json";
#endif

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }

    //This creates the directory folder
    public static bool DirectoryStuff(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);

        try
        {
            if (di.Exists)
            {
                //Debug.Log("Directory Exists Yay");
                return true;

            }
            else
            {
                di.Create();
                //Debug.Log("Directory Created");
                return false;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"The process failed: {ex.ToString()})");
            return false;
        }
    }

    public static bool CreateSubDirectorys(string path)
    {
        string baseLevels = path + "BaseLevels/";
        string customLevels = path + "CustomLevels/";

        DirectoryInfo di = new DirectoryInfo(baseLevels);
        di.Create();
        DirectoryInfo di2 = new DirectoryInfo(customLevels);
        di2.Create();

        return false;
    }

    public static SaveNames CheckGetSaves(string path)
    {
        SaveNames result = new SaveNames();

        //Check if Directory Already Exists
        //Directory is created if it didn't exist prior
        if(DirectoryStuff(path))
        {
            //Writing Over BaseLevels
            ReWriteBaseLevels(path, ref result);
        }
        else
        {
            CreateSubDirectorys(path);
        }

        GetCustomLevels(path, ref result);


        return result;

    }

    //As long as the base directory is made this function is safe to call
    public static void ReWriteBaseLevels(string path, ref SaveNames saves)
    {
        //---
        //Deleting and rebuilding baselevels directory
        string baseLevels = path + "BaseLevels/";
        DirectoryInfo di = new DirectoryInfo(baseLevels);
        if(di.Exists)
        {
            di.Delete(true);
        }

        di.Create();
        //---
        //Loading Resource save names 
        List<TextAsset> songSaves = new List<TextAsset>();
        songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();

        foreach(TextAsset file in songSaves)
        {
            saves.baseLevels.Add(file.name);
        }
        //---

        ///TRANSPORTING DATA TO PERSISTENT PATH
        foreach (TextAsset file in songSaves)
        {
            string jsonData = file.text;
            string savePath = baseLevels + file.name + ".json";
            System.IO.File.WriteAllText(savePath, jsonData);
        }
    }

    public static void GetCustomLevels(string path, ref SaveNames saves)
    {
        string customLevels = path + "CustomLevels/";

        if(DirectoryStuff(customLevels))
        {
            foreach (var file in System.IO.Directory.GetFiles(customLevels))
            {
                //Formatting Save file names correctly via their data path
                string filePath = file.Replace(customLevels, "");
                filePath = filePath.Replace(".json", "");
                saves.customLevels.Add(filePath);
            }
        }
    }

}
