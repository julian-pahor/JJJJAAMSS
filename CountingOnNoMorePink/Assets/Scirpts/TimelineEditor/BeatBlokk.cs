using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatBlokk : MonoBehaviour
{
    public List<BeatSlot> slots = new List<BeatSlot>();
    // Start is called before the first frame update

    public Image imig;
    public Color baseColour;
    public Color beatColour;

    public void Initialise(TimelineEditor editor)
    {
        foreach (BeatSlot slot in slots)
        {
            slot.editor = editor;
        }
    }

    public void Updoot()
    {
        foreach (BeatSlot slot in slots)
        {
            slot.UpdateSlot();
        }
    }

    public IEnumerator Couroot()
    {
        foreach(BeatSlot slot in slots)
        {
            if(slot.uiCard.gameObject.activeSelf)
            {
                Tween t = slot.uiCard.TurnOff();

                if(t != null)
                {
                    yield return t.WaitForCompletion();
                }
            }
        }

        foreach (BeatSlot slot in slots)
        {
            AttackEvent attackEvent = slot.GetSlotEvent();

            if(attackEvent != null)
            {
                slot.uiCard.LoadDisplay(attackEvent);
                Tween t = slot.uiCard.OnLoad();
                if (t != null)
                {
                    yield return t.WaitForCompletion();
                }
            }
        }
    }
}
