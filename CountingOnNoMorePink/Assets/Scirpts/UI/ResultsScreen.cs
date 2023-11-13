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

    private float parryScore;
    private float hitScore;
    private float restartScore;
    private bool perfectFlag = false;

    private float totalWeighting;

    public Sprite pGrade, sGrade, aGrade, bGrade, cGrade;


    // Start is called before the first frame update
    void Start()
    {
        Deactivate();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Activate()
    {
        parryResults.resultAmount.text = "";
        hitResults.resultAmount.text = "";
        restartResults.resultAmount.text = "";

        finalGrade.color = new Color(1, 1, 1, 0);

        CalculateScore();
        SetDisplay();

        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);

        

        gameObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InSine).OnComplete(() => 
        { 
            pSeq = TweenResults(parryResults);
            hSeq = TweenResults(hitResults);
            rSeq = TweenResults(restartResults);
            pSeq.Append(hSeq).Append(rSeq);
            pSeq.Play();
        });
    }

    public void Deactivate()
    {
        gameObject.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutSine).OnComplete(() => { gameObject.SetActive(false); });
    }

    public Sequence TweenResults(ResultsBlock rb)
    {
        Sequence thisSequence = DOTween.Sequence();
        thisSequence.Append(rb.title.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.Insert(0.25f, rb.resultAmount.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.Insert(0.5f, rb.gradeScore.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        return thisSequence.Pause();
    }

    private void CalculateScore()
    {
        perfectFlag = false;

        hitScore = Wobbit.instance.persistentData.currentSongHits;
        restartScore = Wobbit.instance.persistentData.currentSongRestarts;

        parryScore = Wobbit.instance.persistentData.currentSongTotalParrys;

        parryScore = (int)((parryScore - Wobbit.instance.persistentData.currentSongMissedParrys) / parryScore * 100f);

        if (parryScore == Wobbit.instance.persistentData.currentSongPerfectParrys)
        {
            parryScore = 100;
            perfectFlag = true;
            return;
        }

        totalWeighting = 100;
    }
    private void SetDisplay()
    {
        hitResults.resultAmount.text = hitScore.ToString();
        switch(hitScore)
        {
            case (0):
                hitResults.gradeScore.sprite = sGrade;
                totalWeighting *= 1.05f;
                break;
            case (1):
                hitResults.gradeScore.sprite = aGrade;
                totalWeighting *= 0.9f;
                break;
            case (2):
                hitResults.gradeScore.sprite = bGrade;
                totalWeighting *= 0.8f;
                break;
            default:
                hitResults.gradeScore.sprite = cGrade;
                totalWeighting *= 0.7f;
                break;
        }


        restartResults.resultAmount.text = restartScore.ToString();
        switch (hitScore)
        {
            case (0):
                restartResults.gradeScore.sprite = sGrade;
                totalWeighting *= 1.05f;
                break;
            case (1):
                restartResults.gradeScore.sprite = aGrade;
                totalWeighting *= 0.9f;
                break;
            case (2):
                restartResults.gradeScore.sprite = bGrade;
                totalWeighting *= 0.8f;
                break;
            default:
                restartResults.gradeScore.sprite = cGrade;
                totalWeighting *= 0.7f;
                break;
        }

        parryResults.resultAmount.text = parryScore.ToString() + "%";
        if(perfectFlag)
        {
            parryResults.gradeScore.sprite = pGrade;
            totalWeighting *= 2f;
        }
        else
        {
            if(parryScore == 100)
            {
                parryResults.gradeScore.sprite = sGrade;
                totalWeighting *= 1.05f;
            }
            else if(parryScore <= 90)
            {
                parryResults.gradeScore.sprite = aGrade;
                totalWeighting *= .9f;
            }
            else if(parryScore <= 75)
            {
                parryResults.gradeScore.sprite = bGrade;
                totalWeighting *= .8f;
            }
            else
            {
                parryResults.gradeScore.sprite = cGrade;
                totalWeighting *= .7f;
            }
        }

        if(totalWeighting >= 150)
        {
            finalGrade.sprite = pGrade;
            return;
        }
        else if (totalWeighting >= 99)
        {
            finalGrade.sprite = sGrade;
            return;
        }
        else if(totalWeighting >= 75)
        {
            finalGrade.sprite = aGrade;
            return;
        }
        else if(totalWeighting >= 50)
        {
            finalGrade.sprite = bGrade;
            return;
        }
        else
        {
            finalGrade.sprite = cGrade;
        }

    }
}
