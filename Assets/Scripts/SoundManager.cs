using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string volumeParameter = "MasterVolume";

    void Start()
    {
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        float savedVolume = /*PlayerPrefs.GetFloat(volumeParameter, 0.8f);*/ 0.8f;
        volumeSlider.value = savedVolume;

        SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value);

        AudioSource[] allAudioSources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = volumeSlider.value;
        }

        Debug.Log($"Громкость установлена: {Mathf.RoundToInt(volumeSlider.value * 100)}%");
    }

    public void SetVolume(float savedVolume)
    {
        PlayerPrefs.SetFloat(volumeParameter, savedVolume);

        AudioSource[] allAudioSources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = savedVolume;
        }

        Debug.Log($"Громкость установлена: {Mathf.RoundToInt(savedVolume * 100)}%");
    }
}
