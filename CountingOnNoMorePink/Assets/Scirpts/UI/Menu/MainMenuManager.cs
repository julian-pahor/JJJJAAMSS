using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public PersistentData persistentData;

    LoadingScreen loadingScreen;
    SongScoreSaver songScoreManager;

    //everything is hardcoded cos graaah
    [Header("Main Menu Objects")]
    //Main menu
    public RectTransform title;
    public RectTransform surtitle;
    public RectTransform gradient;
    public RectTransform avril;
    public List<RectTransform> buttons = new List<RectTransform>();
    public List<Parallax> parallaxers = new List<Parallax>();
    public Vector2 gradientOffset;

    //AudioManagerRef
    public MenuAudioManager audioManager;
    
    [Space(10)]
    [Header("Level Select Menu Objects")]
    //level select
    public RectTransform selectorWheel;
    public RectTransform backButton;
    public RectTransform banner;

    //card
    public RectTransform playCard;
    public RectTransform playCardContent;
    public TextMeshProUGUI playCardTitle;
    public TextMeshProUGUI playCardInfo;
    public Image gradeImage;
    public List<Sprite> gradeSprites = new List<Sprite>();
    bool playCardIsOpen;

    //credits

    //options

    [Space(10)]
    [Header("Containers")]
    public GameObject mainMenu;
    public GameObject levelSelect;

    bool inTransit;
    

    private void Start()
    {
        songScoreManager = GetComponent<SongScoreSaver>();
        loadingScreen = GetComponent<LoadingScreen>();
        EnterMain();
    }

    private void Update()
    {
        

    }
    public void EnterMain()
    {
        if (inTransit)
            return;

        inTransit = true;

        mainMenu.SetActive(true);
        levelSelect.SetActive(false);

        //parallax will mess us up
        foreach (Parallax parallax in parallaxers)
        {
            parallax.enabled = false;
        }

        //set everything of screen
        Vector2 current = gradient.localPosition;
        gradient.localPosition = gradientOffset;
        foreach (RectTransform button in buttons)
        {
            //button.GetComponent<MainMenuButton>().ResetHighlight();
            MainMenuButton mmb = button.GetComponent<MainMenuButton>();
            mmb.ResetHighlight();
            mmb.blip = audioManager.blipEmitter;
            mmb.selG = audioManager.selGEmitter;
            mmb.selB = audioManager.selBEmitter;
            button.localPosition = (Vector2)button.localPosition + new Vector2(1000, 0);
        }
        Vector2 surtitleDesired = surtitle.localPosition;
        surtitle.localPosition = surtitleDesired + new Vector2(-500, 0);

        Vector2 titleDesired = title.localPosition;
        title.localPosition = titleDesired + new Vector2(-500, 0);

        Vector2 avrilDesired = avril.localPosition;
        avril.localPosition = avrilDesired + new Vector2(0, -500);

        //tween stuff
        surtitle.DOLocalMove(surtitleDesired, .25f).SetEase(Ease.InSine).OnComplete(() => { surtitle.GetComponent<Parallax>().enabled = true; });
        title.DOLocalMove(titleDesired, .4f).SetEase(Ease.InSine).OnComplete(() => { title.GetComponent<Parallax>().enabled = true; });
        avril.DOLocalMove(avrilDesired, .5f).SetEase(Ease.OutBounce).OnComplete(() => { avril.GetComponent<Parallax>().enabled = true;});


        //run sequence
        Sequence sequence = DOTween.Sequence();

        //oh no
        sequence.OnComplete(() =>
        {
           // gradient.GetComponent<Parallax>().enabled = true;
            inTransit = false;
        });

        sequence.Append(gradient.DOLocalMove(current, .3f).SetEase(Ease.InSine));
        foreach (RectTransform button in buttons)
        {
            sequence.Append(button.DOLocalMoveX(0, .1f).SetEase(Ease.OutSine));
        }
       
    }


    public void EnterLevelSelect()
    {
        if (inTransit)
            return;

        inTransit=true;
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);

        playCard.transform.localScale = new Vector3(0,1f,1f); //ROTTATED
        playCard.gameObject.SetActive(false);
        playCardContent.gameObject.SetActive(false);

        //get banner children

        Sequence sequence = DOTween.Sequence();

        int count = 0;
        float delay = .2f;
        foreach(RectTransform rt in banner)
        {
            count++;
            rt.localScale = Vector3.zero;
            rt.DOScale(Vector3.one, .2f).SetEase(Ease.OutCubic).SetDelay(delay - (delay/count));
        }



        Vector2 selectorDesired = selectorWheel.localPosition;
        selectorWheel.localPosition = selectorDesired + new Vector2(500, 0);

        Vector2 backButtonDesired = backButton.localPosition;
        backButton.localPosition = backButtonDesired + new Vector2(-200, 0);

        //Vector2 playButtonDesired = playButton.localPosition;
        //playButton.localPosition = playButtonDesired + new Vector2(0, -250);

        //Vector2 editorButtonDesired = editorButton.localPosition;
        //editorButton.localPosition = editorButtonDesired + new Vector2(0, -150);

        selectorWheel.DOLocalMove(selectorDesired, .3f).SetEase(Ease.InSine).OnComplete(
            () => selectorWheel.GetComponent<LevelSelectScroller>().Swotch(0));
          
        backButton.DOLocalMove(backButtonDesired,.3f).SetEase(Ease.OutBounce);


       //playButton.DOLocalMove(playButtonDesired, .3f).SetEase(Ease.OutQuad);
       //editorButton.DOLocalMove(editorButtonDesired, .3f).SetEase(Ease.OutQuad);

       backButton.DOLocalMove(backButtonDesired, .5f).SetEase(Ease.OutBounce).OnComplete(() => inTransit = false);

    }


    public void OpenPlayCard(string levelName)
    {
        if (inTransit)
            return;
        inTransit = true;

        //IT'S ROTATED THIS WILL HECK UP WHEN YOU CHANGE THE IMAGE

        if(!playCardIsOpen)
        {
            playCardIsOpen = true;
          
            playCard.gameObject.SetActive(true);
            playCard.DOScaleX(1, .2f).SetEase(Ease.OutSine).OnComplete(
                () =>
                {
                    inTransit = false;
                    playCardContent.gameObject.SetActive(true);
                    playCardTitle.text = levelName;

                    SongScoreData data = songScoreManager.LoadSongHighscore(levelName);

                    playCardInfo.text = ScoreInfoFromData(data);


                });
        }
        else
        {
            playCardContent.gameObject.SetActive(false);
            playCard.DOScaleX(0, .2f).SetEase(Ease.OutSine).OnComplete(
              () =>
              {
                  playCardIsOpen=false;
                  inTransit = false;
                  playCard.gameObject.SetActive(false);
              });
        }

        


    }

    public void GoToEditor()
    {
        persistentData.songIndex = selectorWheel.GetComponent<LevelSelectScroller>().GetIndex();
        loadingScreen.BeginLoad("JulesUIBreaking");
    }

    public void StartGame()
    {
        persistentData.songIndex = selectorWheel.GetComponent<LevelSelectScroller>().GetIndex();
        loadingScreen.BeginLoad("AlfRoomOfCretivity");
    }

    public void Quit()
    {
        Application.Quit();
    }

    string ScoreInfoFromData(SongScoreData data)
    {
        string s = "";
        gradeImage.enabled = false;

        if (data != null)
        {
            s += "Hits Taken: " + data.bestHits + "\n";
            s += "Parry Accuracy: " + data.bestTotalParries + "%\n";
            s += "Perfect Parries: " + data.bestPerfectParries + "\n";
            s += "Missed Parries: " + data.bestMissedParries + "\n";

            Sprite grade = GetGradeSprite(data.grade);

            if(grade != null)
            {
                gradeImage.sprite = grade;
                gradeImage.enabled = true;
            }

       
        }
        else
        {
            s += "Hits Taken: " + 0 + "\n";
            s += "Parry Accuracy: " + 0 + "%\n";
            s += "Perfect Parries: " + 0 + "\n";
            s += "Missed Parries: " + 0 + "\n";
          
        }

        return s;
    }

    Sprite GetGradeSprite(string letterGrade)
    {
        switch(letterGrade)
        {
            case "S":
                return gradeSprites[0];
            case "A":
                return gradeSprites[1];
            case "B":
                return gradeSprites[2];
            case "C":
                return gradeSprites[3];
        }
        return null;
    }
}
