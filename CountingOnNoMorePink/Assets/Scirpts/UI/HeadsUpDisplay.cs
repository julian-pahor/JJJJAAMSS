using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour
{
    public Image recordDisc;
    public Slider timelineSlider;

    public Slider beatSliderLeft;
    public Slider beatSliderRight;

    private float sliderValue;
    private float timelineValue;

    private BeatTimeline beatTimeline;

    private Vector3 punchStrength = new Vector3(0.25f, 0.25f, 0.25f);

    // Start is called before the first frame update
    void Start()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger += Beat;
        beatTimeline = Wobbit.instance.timeline;
    }

    private void OnDestroy()
    {
        BeatBroadcast.instance.timelineInfo.onBeatTrigger -= Beat;
    }

    // Update is called once per frame
    void Update()
    {
        sliderValue += Time.deltaTime;
        beatSliderLeft.value = Mathf.Lerp(beatSliderLeft.value, sliderValue, 0.25f);
        beatSliderRight.value = Mathf.Lerp(beatSliderRight.value, sliderValue, 0.25f);

        timelineSlider.value = Mathf.Lerp(timelineSlider.value, timelineValue, 0.01f);

        if (beatTimeline != null)
        {
            timelineValue = beatTimeline.GetSongPercentage();
        }
    }

    
    public void Beat(int m, int b, string measure)
    {
        sliderValue = 0;
        beatSliderLeft.value = 0;
        beatSliderRight.value = 0;
        
        recordDisc.transform.DOPunchScale(punchStrength, 0.175f, 5, 0.5f);


    }
}
