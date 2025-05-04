using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    
    void Start()
    {
        if(!PlayerPrefs.HasKey("Volume")) // Check if the volume key exists in PlayerPrefs
        {
            PlayerPrefs.SetFloat("Volume", 1f); // If not, set it to a default value of 1 (max volume)
        }
        Load(); // Load the saved volume value from PlayerPrefs
    }

    void Update()
    {
        
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save(); 
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f); 
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save(); 
    }
}
