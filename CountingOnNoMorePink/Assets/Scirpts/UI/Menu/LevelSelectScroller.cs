using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelSelectScroller : MonoBehaviour
{

    //um
    //list of like circular buttons or whatever
    //scroll through them and choose one and then set the index and auto load it in the play scene
    //cool

    public Canvas canvas;
    public MainMenuManager manager;

    public LevelButton buttonFab;
    public List<LevelButton> levelList = new List<LevelButton>();
    public int visibleDistance;
    public float spacing;
    public float indent;
    public float transitionSpeed;
    public float buttonScale;

    public int currentIndex;

    bool toggle;
    public TextMeshProUGUI toggleButtonText;

    private void Start()
    {
        LoadBaseSaves();
        SetToggleText("Base Levels");
    }


    public void ToggleSaves()
    {
        toggle = !toggle;

        if (toggle)
        {
            SetToggleText("Base Levels");
            LoadCustomSaves();
        }
        else
        {
            SetToggleText("Custom Levels");
            LoadBaseSaves();
        }

    }


    void SetToggleText(string text)
    {
        if(toggleButtonText == null)
        {
            UnityEngine.Debug.LogWarning("you have not assigned the toggle button text you silly. this is a proper null check");
            return;
        }
        toggleButtonText.text = text;
    }

    void SetPositions()
    {
        currentIndex = levelList.Count - 1;
        for (int i = 0; i < levelList.Count; ++i)
        {

            int distance = Mathf.Abs(i - currentIndex) + 1;

            //let's not discuss how garbage this process is

            levelList[i].gameObject.SetActive(true);
            levelList[i].SetImmediate(GetComponent<RectTransform>().position + new Vector3(distance * indent, (i - currentIndex) * (canvas.scaleFactor * spacing)), (Vector3.one * buttonScale) / distance, Mathf.Abs(i - currentIndex));
        }
    }


    void LoadBaseSaves()
    {
        ClearAllButtons();

        string path = Application.persistentDataPath + "/SongSaves/";

        Utilities.SaveNames saves = new Utilities.SaveNames();

        saves = Utilities.CheckGetSaves(path);

        foreach(string s in saves.baseLevels)
        {
            LevelButton b = Instantiate(buttonFab, transform);
            b.levelTitle.text = s;
            b.GetComponent<Button>().onClick.AddListener(() => manager.OpenPlayCard(b.levelTitle.text)); //who even knows
            //b.GetComponent<Button>().onClick.AddListener((() => manager.audioManager.selGEmitter.Play())); //I dont even know either :'c
            levelList.Add(b);
        }

        levelList.Reverse();
        SetPositions();
    }

    void LoadCustomSaves()
    {
        ClearAllButtons();

        string path = Application.persistentDataPath + "/SongSaves/";

        Utilities.SaveNames saves = new Utilities.SaveNames();

        saves = Utilities.CheckGetSaves(path);

        foreach (string s in saves.customLevels)
        {
            LevelButton b = Instantiate(buttonFab, transform);
            b.levelTitle.text = s;
            b.GetComponent<Button>().onClick.AddListener(() => manager.OpenPlayCard(b.levelTitle.text)); //who even knows
            //b.GetComponent<Button>().onClick.AddListener((() => manager.audioManager.selGEmitter.Play())); //I dont even know either :'c
            levelList.Add(b);
        }

        levelList.Reverse();
        SetPositions();
    }


    void ClearAllButtons()
    {
        foreach(LevelButton b in levelList)
        {
            Destroy(b.gameObject);
        }
        levelList.Clear();
    }

    void TransportFilesFromResources()
    {
        List<TextAsset> songSaves = new List<TextAsset>();
        songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();
        foreach (TextAsset file in songSaves)
        {
            string jsonData = file.text;
            string path = Application.persistentDataPath + "/SongSaves/" + file.name + ".json";
            System.IO.File.WriteAllText(path, jsonData);
        }
    }

    public void Swotch(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, levelList.Count-1);

        for (int i = 0; i < levelList.Count; ++i)
        {

            int distance = Mathf.Abs(i - currentIndex) +1;
            //levelList[i].levelTitle.text = distance.ToString();
            if (distance > visibleDistance)
                levelList[i].gameObject.SetActive(false);
            else
            {
                levelList[i].gameObject.SetActive(true);
                levelList[i].Move(GetComponent<RectTransform>().position + new Vector3(distance * indent,(i-currentIndex) * (canvas.scaleFactor * spacing)), (Vector3.one * buttonScale) / distance, transitionSpeed, Mathf.Abs(i - currentIndex));
                levelList[i].GetComponent<Button>().interactable = distance == 1;
            }
        }
    }

    //this is reversed for some reason
    public int GetIndex()
    {
        return (levelList.Count -1) - currentIndex;
    }
}
