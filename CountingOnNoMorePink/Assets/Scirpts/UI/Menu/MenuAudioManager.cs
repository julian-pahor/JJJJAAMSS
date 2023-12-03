using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MenuAudioManager : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter blipEmitter;
    public FMODUnity.StudioEventEmitter selGEmitter;
    public FMODUnity.StudioEventEmitter selBEmitter;

    public static MenuAudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
