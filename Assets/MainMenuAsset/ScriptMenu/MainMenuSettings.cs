using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI;   

public class VolumeController : MonoBehaviour
{
    public AudioMixer mainMixer;
    public string parameterName;
    //public float currentsound = 1;
    public Slider volumeSlider;


    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(parameterName, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }
    public void SetVolume(float sliderValue)
    {
        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        mainMixer.SetFloat(parameterName, volumeInDb);

        PlayerPrefs.SetFloat(parameterName, sliderValue);
        PlayerPrefs.Save();
    }
}