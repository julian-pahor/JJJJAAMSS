using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScroller : MonoBehaviour
{

    //um
    //list of like circular buttons or whatever
    //scroll through them and choose one and then set the index and auto load it in the play scene
    //cool

    public List<LevelButton> levelList = new List<LevelButton>();
    public int currentIndex;
    public int visibleDistance;
    public int spacing;
    public float transitionSpeed;

    public void Swotch(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, levelList.Count-1);

        for (int i = 0; i < levelList.Count; ++i)
        {

            int distance = Mathf.Abs(i - currentIndex) +1;
            levelList[i].levelTitle.text = distance.ToString();
            if (distance > visibleDistance)
                levelList[i].gameObject.SetActive(false);
            else
            {
                levelList[i].gameObject.SetActive(true);
                levelList[i].Move(GetComponent<RectTransform>().anchoredPosition + ((i - currentIndex) * Vector2.one * spacing), Vector3.one / distance, transitionSpeed);
            
            }
        }

    }
}
