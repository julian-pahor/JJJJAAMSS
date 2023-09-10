using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBlokk : MonoBehaviour
{
    public List<BeatSlot> slots = new List<BeatSlot>();
    // Start is called before the first frame update

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


}
