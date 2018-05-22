using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Slider slider;
    public Text musicVolume;

    public void Update()
    {
        AudioListener.volume = slider.value;
        musicVolume.text = (slider.value * 100f).ToString("F0") + "%";
    }
}
