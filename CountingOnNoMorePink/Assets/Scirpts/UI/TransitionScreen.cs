using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class TransitionScreen : MonoBehaviour
{
    public Image blackScreen;
    public TextMeshProUGUI countIn;
    public TextMeshProUGUI countInShadow;

    private void Start()
    {
        float punchTime = .8f;
        int vie_brayto = 4;
        float vie_crayto =.2f;

        SwapText("3");

        BeatTimeline beatTimeLine = Wobbit.instance.GetComponent<BeatTimeline>();

        countIn.gameObject.SetActive(false);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(blackScreen.transform.DOScale(Vector2.zero, 1f).SetEase(Ease.OutBounce).OnComplete(() => countIn.gameObject.SetActive(true)));

        sequence.Append(countIn.transform.DOPunchScale(Vector3.one * 1.5f, punchTime,vie_brayto,vie_crayto).OnComplete(() => SwapText("2")));

        sequence.Append(countIn.transform.DOPunchScale(Vector3.one * 1.5f, punchTime, vie_brayto, vie_crayto).OnComplete(() => SwapText("1")));

        sequence.Append(countIn.transform.DOPunchScale(Vector3.one * 1.5f, punchTime, vie_brayto, vie_crayto).OnComplete(() =>
            {
                beatTimeLine.StartGame();
                gameObject.SetActive(false);
            }));



    }

    void SwapText(string text)
    {
        countIn.text = text;
        countInShadow.text = text;
    }    
}
