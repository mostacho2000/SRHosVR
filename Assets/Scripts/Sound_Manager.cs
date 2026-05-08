using UnityEngine;
using UnityEngine.UI;

public class Sound_Manager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource ambientSource;

    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider ambientSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string AmbientVolumeKey = "AmbientVolume";

    void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            float savedMusic = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            musicSlider.value = savedMusic;
            SetMusicVolume(savedMusic);
        }
        else
        {
            Debug.LogWarning("[Sound_Manager] musicSlider no está asignado.", this);
        }

        if (ambientSlider != null)
        {
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
            float savedAmbient = PlayerPrefs.GetFloat(AmbientVolumeKey, 1f);
            ambientSlider.value = savedAmbient;
            SetAmbientVolume(savedAmbient);
        }
        else
        {
            Debug.LogWarning("[Sound_Manager] ambientSlider no está asignado.", this);
        }
    }

    public void SetMusicVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = clamped;
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, clamped);
    }

    public void SetAmbientVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        if (ambientSource != null)
        {
            ambientSource.volume = clamped;
        }
        PlayerPrefs.SetFloat(AmbientVolumeKey, clamped);
    }
}
