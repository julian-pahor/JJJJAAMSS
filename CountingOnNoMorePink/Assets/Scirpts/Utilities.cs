using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public static class Utilities
{
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

#if UNITY_EDITOR
        string path = $"Assets/Resources/SongSaves/{saveName}.json";

#else
        string path = Application.persistentDataPath + "/" + saveName + ".json";
#endif


        string jsonData = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(path, jsonData);
    }

    public static GameData LoadData(string saveName)
    {
#if UNITY_EDITOR
        string path = $"Assets/Resources/SongSaves/{saveName}.json";

#else
        string path = Application.persistentDataPath + "/" + saveName + ".json";
#endif

        if (!System.IO.File.Exists(path)) return null;

        path = System.IO.File.ReadAllText(path);
        GameData thisData = new GameData();
        thisData = JsonUtility.FromJson<GameData>(path);

        return thisData;
    }

    public static void DeleteData(string saveName)
    {
#if UNITY_EDITOR
        string path = $"Assets/Resources/SongSaves/{saveName}.json";

#else
        string path = Application.persistentDataPath + "/" + saveName + ".json";
#endif

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}
