using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixerSliders : MonoBehaviour
{
    public FMOD.Studio.Bus bus;

    public string busName;

    public float busVolume;

    public Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/" + busName);
        bus.getVolume(out busVolume);
        slider.value = busVolume;
    }

    // Update is called once per frame
    void Update()
    {
        bus.setVolume(DecibelToLinear(busVolume));
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20f);
        return linear;
    }

    public void SetVolume(float f)
    {
        busVolume = f;
    }
}
