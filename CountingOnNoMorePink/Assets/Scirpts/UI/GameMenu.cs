using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameMenu : MonoBehaviour
{
    public RectTransform menu;
    bool menuIsActive;
    public float startX;
    public float targetX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (!menuIsActive)
        {
            //Wobbit.instance.paused = true;
            menuIsActive = true;
            menu.DOLocalMoveX(targetX, .15f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                //Time.timeScale = 0;
                //BeatBroadcast.instance.Pause();
            });
        }
        else
        {
            //Time.timeScale = 1;
            //Wobbit.instance.paused = false;
            //BeatBroadcast.instance.Resume();
            menuIsActive = false;
            menu.DOLocalMoveX(startX, .15f).SetEase(Ease.InSine);

        }
    }
}
