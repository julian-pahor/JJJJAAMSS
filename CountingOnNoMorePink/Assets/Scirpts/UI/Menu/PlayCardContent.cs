using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayCardContent : MonoBehaviour
{
    public List<Sprite> gradeSprites = new List<Sprite> ();

    [Header("ParryBlock")]
    public TextMeshProUGUI parryStat;
    public Image parryGrade;

    [Header("HitsBlock")]
    public TextMeshProUGUI hitStat;
    public Image hitGrade;

    [Header("RestartsBlock")]
    public TextMeshProUGUI restartStat;
    public Image restartGrade;

    [Space(5)]
    public Image overallGrade;

    public void SetUpCard(SongScoreData data)
    {
        if(data == null)
        {
            //no score settings
            parryStat.text = "-";
            hitStat.text = "-";
            restartStat.text = "-";

            parryGrade.sprite = gradeSprites[4];
            hitGrade.sprite = gradeSprites[4];
            restartGrade.sprite = gradeSprites[4];

            overallGrade.sprite = gradeSprites[4];
        }
        else
        {
            //actual data being read
            parryStat.text = data.bestTotalParries.ToString() + "%";

            if (data.bestTotalParries == 100)
            {
                parryGrade.sprite = gradeSprites[0];
            }
            else if (data.bestTotalParries >= 90)
            {
                parryGrade.sprite = gradeSprites[1];
            }
            else if (data.bestTotalParries >= 75)
            {
                parryGrade.sprite = gradeSprites[2];
            }
            else
            {
                parryGrade.sprite = gradeSprites[3];
            }

            hitStat.text = data.bestHits.ToString();

            switch (data.bestHits)
            {
                case (0):
                    hitGrade.sprite = gradeSprites[0];
                    break;
                case (1):
                    hitGrade.sprite = gradeSprites[1];
                    break;
                case (2):
                    hitGrade.sprite = gradeSprites[2];
                    break;
                default:
                    hitGrade.sprite = gradeSprites[3];
                    break;
            }


            restartStat.text = data.attempts.ToString();

            switch (data.attempts)
            {
                case (0):
                    restartGrade.sprite = gradeSprites[0];
                    break;
                case (1):
                    restartGrade.sprite = gradeSprites[1];
                    break;
                case (2):
                    restartGrade.sprite = gradeSprites[2];
                    break;
                default:
                    restartGrade.sprite = gradeSprites[3];
                    break;
            }

            overallGrade.sprite = GetGradeSprite(data.grade);
        }
    }

    Sprite GetGradeSprite(string letterGrade)
    {
        switch (letterGrade)
        {
            case "S":
                return gradeSprites[0];
            case "A":
                return gradeSprites[1];
            case "B":
                return gradeSprites[2];
            case "C":
                return gradeSprites[3];
            default:
                return gradeSprites[4];
        }
    }
}
