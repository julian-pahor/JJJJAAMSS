using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
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
    
    [Space(10)]
    [Header("Level Select Menu Objects")]
    //level select
    public RectTransform selectorWheel;
    public RectTransform backButton;


    //credits

    //options

    [Space(10)]
    [Header("Containers")]
    public GameObject mainMenu;
    public GameObject levelSelect;

    bool inTransit;

    private void Start()
    {

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
            button.GetComponent<MainMenuButton>().ResetHighlight();
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

        Vector2 selectorDesired = selectorWheel.localPosition;
        selectorWheel.localPosition = selectorDesired + new Vector2(500, 0);

        Vector2 backButtonDesired = backButton.localPosition;
        backButton.localPosition = backButtonDesired + new Vector2(-200, 0);

        selectorWheel.DOLocalMove(selectorDesired, .3f).SetEase(Ease.InSine).OnComplete(
            () => selectorWheel.GetComponent<LevelSelectScroller>().Swotch(0));
          
        backButton.DOLocalMove(backButtonDesired,.3f).SetEase(Ease.OutBounce).OnComplete(() => inTransit = false);



    }


    public void PlayButton()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("AlfRoomOfCretivity");
    }

}
