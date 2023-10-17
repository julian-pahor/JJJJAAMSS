using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    public ResultsBlock parryResults;
    private Sequence pSeq;

    public ResultsBlock hitResults;
    private Sequence hSeq;

    public ResultsBlock restartResults;
    private Sequence rSeq;

    public Image finalGrade;



    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: Replace with nice looking cascading lerps
    public void Activate()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);

        parryResults.resultAmount.text = "";
        hitResults.resultAmount.text = "";
        restartResults.resultAmount.text = "";

        gameObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InSine).OnComplete(() => 
        { 
            pSeq = TweenResults(parryResults, 100f);
            hSeq = TweenResults(hitResults, 2f);
            rSeq = TweenResults(restartResults, 0f);
            pSeq.Append(hSeq).Append(rSeq);
            pSeq.Play();
        });
    }

    public void Deactivate()
    {
        gameObject.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutSine).OnComplete(() => { gameObject.SetActive(false); });
    }

    public Sequence TweenResults(ResultsBlock rb, float f)
    {
        Sequence thisSequence = DOTween.Sequence();
        thisSequence.Append(rb.title.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.Insert(0.1f, rb.resultAmount.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.Insert(0.2f, rb.gradeScore.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.InsertCallback(0.1f, () => { rb.resultAmount.text = f.ToString(); });
        return thisSequence.Pause();
    }
}
