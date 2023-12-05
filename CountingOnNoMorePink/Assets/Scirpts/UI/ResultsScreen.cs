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

    public Sprite sGrade, aGrade, bGrade, cGrade;
    public Dictionary<Sprite, string> letterGradings = new Dictionary<Sprite, string>();


    // Start is called before the first frame update
    void Start()
    {
        //yeh I made a whole dictionary for four letters WHADRYDYE GONNAE DO HUH
        letterGradings.Add(sGrade, "S");
        letterGradings.Add(aGrade, "A");
        letterGradings.Add(bGrade, "B");
        letterGradings.Add(cGrade, "C");

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

        parryResults.gradeScore.color = new Color(1,1,1,0);
        hitResults.gradeScore.color = new Color(1, 1, 1, 0);
        restartResults.gradeScore.color = new Color(1, 1, 1, 0);

        finalGrade.transform.localScale = Vector3.zero;

        //CalculateScore();
        SetDisplay();

        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);

        

        gameObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InSine).OnComplete(() => 
        { 
            pSeq = TweenResults(parryResults);
            hSeq = TweenResults(hitResults);
            rSeq = TweenResults(restartResults);
            pSeq.Append(hSeq).Append(rSeq);
            pSeq.Play().OnComplete(() =>
            {
                finalGrade.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InQuad);
            });
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
        thisSequence.Insert(0.45f, rb.gradeScore.DOFade(1, 0.1f).SetEase(Ease.InQuad));
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
            totalWeighting = 100;
            return;
        }

        totalWeighting = 100;
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

        letterGradings.TryGetValue(finalGrade.sprite, out dat.grade);

        return dat;
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
        switch (restartScore)
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
            parryResults.gradeScore.sprite = sGrade;
            totalWeighting *= 2f;
        }
        else
        {
            if(parryScore == 100)
            {
                parryResults.gradeScore.sprite = sGrade;
                totalWeighting *= 1.05f;
            }
            else if(parryScore >= 90)
            {
                parryResults.gradeScore.sprite = aGrade;
                totalWeighting *= .9f;
            }
            else if(parryScore >= 75)
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

        if (totalWeighting >= 99)
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

    public IEnumerator AnimDelay()
    {
        CalculateScore();
        yield return new WaitForSeconds(3.5f);
        Activate();
    }
}
