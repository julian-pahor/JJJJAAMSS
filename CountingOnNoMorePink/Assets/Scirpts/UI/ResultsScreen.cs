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
        thisSequence.Insert(0.2f, rb.resultAmount.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        thisSequence.Insert(0.3f, rb.gradeScore.transform.DOPunchScale(Vector3.one * 1.15f, 0.25f));
        return thisSequence.Pause();
    }

    private void CalculateScore()
    {
        perfectFlag = false;

        hitScore = Wobbit.instance.persistentData.currentSongHits;
        restartScore = Wobbit.instance.persistentData.currentSongRestarts;

        parryScore = Wobbit.instance.persistentData.currentSongTotalParrys;

        if(parryScore == Wobbit.instance.persistentData.currentSongPerfectParrys)
        {
            parryScore = 100;
            perfectFlag = true;
            return;
        }

        parryScore = ((parryScore - Wobbit.instance.persistentData.currentSongMissedParrys) / parryScore * 100f);
    }

    //AUUAAGAUAAGAUGUUUGHGHG
    public SongScoreData GetScoreData()
    {
        SongScoreData dat = new SongScoreData();
        dat.attempts = (int)restartScore;
        dat.bestHits = (int)hitScore;
        dat.bestTotalParries = (int)parryScore;
        dat.bestMissedParries = Wobbit.instance.persistentData.currentSongMissedParrys;
        dat.bestPerfectParries = Wobbit.instance.persistentData.currentSongPerfectParrys;

        return dat;
    }

    private void SetDisplay()
    {
        hitResults.resultAmount.text = hitScore.ToString();
        switch(hitScore)
        {
            case (0):
                hitResults.gradeScore.sprite = sGrade;
                break;
            case (1):
                hitResults.gradeScore.sprite = aGrade;
                break;
            case (2):
                hitResults.gradeScore.sprite = bGrade;
                break;
            default:
                hitResults.gradeScore.sprite = cGrade;
                break;
        }


        restartResults.resultAmount.text = restartScore.ToString();
        switch (hitScore)
        {
            case (0):
                restartResults.gradeScore.sprite = sGrade;
                break;
            case (1):
                restartResults.gradeScore.sprite = aGrade;
                break;
            case (2):
                restartResults.gradeScore.sprite = bGrade;
                break;
            default:
                restartResults.gradeScore.sprite = cGrade;
                break;
        }

        parryResults.resultAmount.text = parryScore.ToString() + "%";
        if(perfectFlag)
        {
            parryResults.gradeScore.sprite = pGrade;
            return;
        }
        else
        {
            if(parryScore == 100)
            {
                parryResults.gradeScore.sprite = sGrade;
            }
            else if(parryScore <= 90)
            {
                parryResults.gradeScore.sprite = aGrade;
            }
            else if(parryScore <= 75)
            {
                parryResults.gradeScore.sprite = bGrade;
            }
            else
            {
                parryResults.gradeScore.sprite = cGrade;
            }
        }
    }
}
