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

    // Play methods
    public void PlaySound(AudioClip clip, AudioChannel channel, bool looped = false)
    {
        switch (channel)
        {
            case AudioChannel.Music:
                if (!isMusicMuted)
                {
                    musicChannel.clip = clip;
                    musicChannel.loop = looped;
                    musicChannel.Play();
                }
                break;
            case AudioChannel.UI:
                if (!isUIEffectsMuted)
                {
                    uiEffectsChannel.clip = clip;
                    uiEffectsChannel.loop = looped;
                    uiEffectsChannel.Play();
                }
                break;
            case AudioChannel.SFX:
                if (!isSFXMuted)
                {
                    sfxChannel.clip = clip;
                    sfxChannel.loop = looped;
                    sfxChannel.Play();
                }
                break;
        }
    }

    // Stop methods
    public void StopSound(AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Music:
                musicChannel.Stop();
                break;
            case AudioChannel.UI:
                uiEffectsChannel.Stop();
                break;
            case AudioChannel.SFX:
                sfxChannel.Stop();
                break;
        }
    }

    // Method to stop all audio
    public void StopAllSound()
    {
        musicChannel.Stop();
        uiEffectsChannel.Stop();
        sfxChannel.Stop();
    }

}
