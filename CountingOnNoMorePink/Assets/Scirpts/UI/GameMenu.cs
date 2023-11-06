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
            menuIsActive = true;
            menu.DOLocalMoveX(targetX, .2f).SetEase(Ease.OutSine);
        }
        else
        {
            menuIsActive = false;
            menu.DOLocalMoveX(startX, .2f).SetEase(Ease.InSine);

        }
    }
}
