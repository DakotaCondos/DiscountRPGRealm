using UnityEngine;
public enum AudioChannel
{
    Music,
    SFX,
    UI
}

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
    public bool IsMusicMuted { get => isMusicMuted; }
    public bool IsUIEffectsMuted { get => isUIEffectsMuted; }
    public bool IsSFXMuted { get => isSFXMuted; }



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
        isMusicMuted = !IsMusicMuted;
        musicChannel.mute = IsMusicMuted;
    }

    public void ToggleUIEffectsMute()
    {
        isUIEffectsMuted = !IsUIEffectsMuted;
        uiEffectsChannel.mute = IsUIEffectsMuted;
    }

    public void ToggleSFXMute()
    {
        isSFXMuted = !IsSFXMuted;
        sfxChannel.mute = IsSFXMuted;
    }
}
