using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
//using Unity.VisualScripting;

public class MainMenuManager : MonoBehaviour
{
    public PersistentData persistentData;

    LoadingScreen loadingScreen;
    SongScoreSaver songScoreManager;

    //everything is hardcoded cos graaah
    [Header("Main Menu Objects")]
    //Main menu
    public RectTransform title;

    public GameObject avrilModel;
    public GameObject mainCamera;

    //public RectTransform surtitle;
    public List<RectTransform> buttons = new List<RectTransform>();
    public List<Parallax> parallaxers = new List<Parallax>();
    public Vector2 gradientOffset;

    [Space(10)]
    [Header("Level Select Menu Objects")]
    //level select
    public Transform lerpEndTarget;
    public Transform lerpQuadTarget;

    public GameObject book;
    public RectTransform selectorWheel;
    public RectTransform backButton;
    public RectTransform banner;

    //Comic Holder
    public List<RectTransform> comicPanels = new List<RectTransform>();
    public Renderer bookRenderer;
    public Material bookMatPage;
    public Material bookMatNoPage;

    //card
    public RectTransform playCard;
    public RectTransform playCardContent;
    public TextMeshProUGUI playCardTitle;
    public Button playCardBackButton;
    public PlayCardContent playCardData;
    bool playCardIsOpen;


    //Options Menu Stuff
    [Space(10)]
    [Header("Options Menu Sliders")]
    public RectTransform settingsContent;

    [Space(10)]
    [Header("Credits Holder")]
    public RectTransform creditsHolder;

    //QuadLerpGarbage
    private Vector3 startPosition;
    private Quaternion startRotation;

    //credits

    //options

    [Space(10)]
    [Header("Containers")]
    public GameObject mainMenu;
    public GameObject levelSelect;

    bool inTransit;

    [Range(0, 1)]
    public float lerpSlider;

    private void Start()
    {
        songScoreManager = GetComponent<SongScoreSaver>();
        loadingScreen = GetComponent<LoadingScreen>();
        levelSelect.SetActive(false);
        settingsContent.gameObject.SetActive(false);
        creditsHolder.gameObject.SetActive(false);
        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;

        //var seq = DOTween.Sequence()
        //        .Append(avrilModel.transform.DOScale(new Vector3(avrilModel.transform.localScale.x, avrilModel.transform.localScale.y * 1.2f, avrilModel.transform.localScale.z), 0.462f).SetEase(Ease.OutBack));

        //seq.SetLoops(-1, LoopType.Yoyo);

        EnterMain();
    }

    private void Update()
    {

        //bookRenderer.material.Lerp(bookMatPage, bookMatNoPage, lerpSlider);
    }

    [ContextMenu("EnterMain")]
    public void EnterMain()
    {
        if (inTransit)
            return;

        inTransit = true;

        mainMenu.SetActive(true);
        

        //run sequence
        Sequence sequence = DOTween.Sequence();

        //oh no
        sequence.OnComplete(() =>
        {
            inTransit = false;
        });

        //parallax will mess us up
        foreach (Parallax parallax in parallaxers)
        {
            parallax.enabled = false;
        }

        sequence.Append(title.DOLocalMove((Vector2)title.localPosition + new Vector2(0, 250), .6f).From().SetEase(Ease.OutQuad).OnComplete(() => { title.GetComponent<Parallax>().enabled = true; }));
        //sequence.Insert(0.45f, title.DOPunchScale(new Vector3(0.75f, 0.45f, 0.75f), 0.6f, 0, 0).SetEase(Ease.OutBack));
 

        int indexer = 0;

        //set everything of screen
        foreach (RectTransform button in buttons)
        {
            MainMenuButton mmb = button.GetComponent<MainMenuButton>();
            mmb.ResetHighlight();

            sequence.Append(button.DOLocalMove((Vector2)button.localPosition + new Vector2(indexer == 0 ? -1000 : 500, 0), 0.2f).From().SetEase(Ease.OutBack));

            indexer++;
            
            if(indexer >= 2)
            {
                indexer = 0;
            }
        }       
    }

    public void CloseMenu()
    {
        if (inTransit)
            return;

        inTransit = true;

        foreach (Parallax parallax in parallaxers)
        {
            parallax.enabled = false;
        }

        //run sequence
        Sequence sequence = DOTween.Sequence();

        //oh no
        sequence.OnComplete(() =>
        {
            inTransit = false;
            mainMenu.SetActive(false);
            title.gameObject.SetActive(true);

            foreach(RectTransform button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        });

        Vector3 titleStart = title.localPosition;

        sequence.Append(title.DOLocalMove((Vector2)title.localPosition + new Vector2(0, 250), .25f).SetEase(Ease.InBack).OnComplete(() => { title.gameObject.SetActive(false); title.localPosition = titleStart; }));

        int indexer = 0;

        //set everything of screen
        foreach (RectTransform button in buttons)
        {
            MainMenuButton mmb = button.GetComponent<MainMenuButton>();
            mmb.ResetHighlight();

            Vector3 buttonPos = button.localPosition;
            sequence.Append(button.DOLocalMove((Vector2)button.localPosition + new Vector2(indexer == 0 ? -1000 : 500, 0), 0.2f).SetEase(Ease.OutBack).OnComplete(() => { button.gameObject.SetActive(false); button.localPosition = buttonPos; }));

            indexer++;

            if (indexer >= 2)
            {
                indexer = 0;
            }
        }
    }


    public void EnterLevelSelect()
    {

        if (inTransit)
            return;

        CloseMenu();

        StartCoroutine(CameraMove(true));
    }

    public void SetUpLevelSelect()
    {
        levelSelect.SetActive(true);
        selectorWheel.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        playCard.gameObject.SetActive(false);
        playCardContent.gameObject.SetActive(false);

        int count = 0;
        float delay = .25f;
        //Comic Pop ups
        foreach (RectTransform rt in banner)
        {
            count++;
            rt.localScale = Vector3.zero;
            rt.DOScale(Vector3.one, delay).SetEase(Ease.OutCubic).SetDelay(delay - (delay / count));
        }

        selectorWheel.DOLocalMove((Vector2)selectorWheel.localPosition + new Vector2(500, 0), .3f).From().SetEase(Ease.InSine).OnComplete(
            () => selectorWheel.GetComponent<LevelSelectScroller>().Swotch(0));

        backButton.DOLocalMove((Vector2)backButton.localPosition + new Vector2(-200, 0), .3f).From().SetEase(Ease.OutBounce);
    }

    public void CloseLevelSelect()
    {
        int count = 0;
        float delay = .2f;
        //Comic Pop ups
        foreach (RectTransform rt in banner)
        {
            count++;
            rt.DOScale(Vector3.zero, delay).SetEase(Ease.OutCubic).SetDelay(delay - (delay / count));
        }

        Vector3 selecterPos = selectorWheel.localPosition;
        Vector3 backPos = backButton.localPosition;

        selectorWheel.DOLocalMove((Vector2)selectorWheel.localPosition + new Vector2(500, 0), .3f).SetEase(Ease.InSine).OnComplete(() =>
        {
            selectorWheel.gameObject.SetActive(false);
            selectorWheel.localPosition = selecterPos;
        });

        backButton.DOLocalMove((Vector2)backButton.localPosition + new Vector2(-200, 0), .3f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            backButton.gameObject.SetActive(false);
            backButton.localPosition = backPos;
        });

        StartCoroutine(CameraMove(false));
    }

    //going = true = moving toward book
    private IEnumerator CameraMove(bool going)
    {
        yield return null;
        float lerp = 0;

        if (going)
        {
            bookRenderer.material = bookMatNoPage;
            while(lerp < 1)
            {
                mainCamera.transform.position = Utilities.QuadraticLerp(startPosition, lerpQuadTarget.position, lerpEndTarget.position, lerp);
                mainCamera.transform.rotation = Quaternion.Lerp(startRotation, lerpEndTarget.transform.rotation, lerp);

                //mainCamera.transform.LookAt(book.transform);

                //if (lerp < 0.975f)
                //{
                //    mainCamera.transform.LookAt(book.transform);
                //}
                //else
                //{
                //    mainCamera.transform.rotation = Quaternion.Lerp(startRotation, lerpEndTarget.transform.rotation, lerp);
                //}

                lerp += Time.deltaTime;
                yield return null;
            }

            SetUpLevelSelect();
            
        }
        else
        {
            bookRenderer.material = bookMatPage;

            while (lerp < 1)
            {
                mainCamera.transform.position = Utilities.QuadraticLerp(lerpEndTarget.position, lerpQuadTarget.position, startPosition, lerp);
                mainCamera.transform.rotation = Quaternion.Lerp(lerpEndTarget.transform.rotation, startRotation, lerp);

                lerp += Time.deltaTime;
                yield return null;
            }

            EnterMain();
        }
    }


    public void OpenPlayCard(string levelName)
    {
        if (inTransit)
            return;
        inTransit = true;

        playCardBackButton.onClick.RemoveAllListeners();

        if(!playCardIsOpen)
        {
            playCardIsOpen = true;
          playCardBackButton.onClick.AddListener(() => OpenPlayCard(levelName));
            playCard.gameObject.SetActive(true);
            playCard.DOLocalMove((Vector2)playCard.localPosition + new Vector2(1000, 0), .35f).From().SetEase(Ease.OutBack).OnComplete(
                () =>
                {
                    inTransit = false;
                    playCardContent.gameObject.SetActive(true);
                    playCardTitle.text = levelName;

                    SongScoreData data = songScoreManager.LoadSongHighscore(levelName);

                    playCardData.SetUpCard(data);
                });
        }
        else
        {
            Vector3 playPos = playCard.localPosition;
            playCardContent.gameObject.SetActive(false);
            playCard.DOLocalMove((Vector2)playCard.localPosition + new Vector2(1000, 0), .35f).OnComplete(
              () =>
              {
                  playCardIsOpen=false;
                  inTransit = false;
                  playCard.gameObject.SetActive(false);
                  playCard.localPosition = playPos;
              });
        }
    }

    public void OpenSettings()
    {

        if (inTransit)
            return;
        inTransit = true;

        settingsContent.gameObject.SetActive(true);
        settingsContent.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.OutBack).OnComplete(() => {
            inTransit = false;
        });
    }

    public void CloseSettings()
    {
        if (inTransit)
            return;
        inTransit = true;

        settingsContent.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
            settingsContent.gameObject.SetActive(false);
            settingsContent.localScale = Vector3.one;
            inTransit = false;
        });
    }

    public void OpenCredits()
    {
        if (inTransit)
            return;
        inTransit = true;

        creditsHolder.gameObject.SetActive(true);
        creditsHolder.DOScale(Vector3.zero, 0.47f).From().SetEase(Ease.OutBack).OnComplete(() => {
            inTransit = false;
        });

    }

    public void CloseCredits()
    {
        if (inTransit)
            return;
        inTransit = true;

        creditsHolder.DOScale(Vector3.zero, 0.333f).SetEase(Ease.InBack).OnComplete(() => {
            creditsHolder.gameObject.SetActive(false);
            creditsHolder.localScale = Vector3.one;
            inTransit = false;
        });


    }

    public void GoToEditor()
    {
        persistentData.songIndex = selectorWheel.GetComponent<LevelSelectScroller>().GetIndex();
        loadingScreen.BeginLoad("FinalTimelineEditor");
    }

    public void StartGame()
    {
        persistentData.songIndex = selectorWheel.GetComponent<LevelSelectScroller>().GetIndex();
        loadingScreen.BeginLoad("PossibleFinalCandyScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
