using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicChannel;
    [SerializeField]
    private AudioSource uiEffectsChannel;
    [SerializeField]
    private AudioSource sfxChannel;

    private bool isMusicMuted = false;
    private bool isUIEffectsMuted = false;
    private bool isSFXMuted = false;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Volume setters
    public void SetMusicVolume(float volume)
    {
        musicChannel.volume = volume;
    }

    public void SetUIEffectsVolume(float volume)
    {
        uiEffectsChannel.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxChannel.volume = volume;
    }

    // Mute toggles
    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        musicChannel.mute = isMusicMuted;
    }

    public void ToggleUIEffectsMute()
    {
        isUIEffectsMuted = !isUIEffectsMuted;
        uiEffectsChannel.mute = isUIEffectsMuted;
    }

    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        sfxChannel.mute = isSFXMuted;
    }
}
