using Nova;
using UnityEngine;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] UIBlock2D unMuted;
    [SerializeField] UIBlock2D muted;
    private bool isMuted = false;
    [SerializeField] AudioChannel audioChannel = AudioChannel.Music;

    private void Start()
    {
        switch (audioChannel)
        {
            case AudioChannel.Music:
                isMuted = AudioManager.Instance.IsMusicMuted;
                break;

            case AudioChannel.UI:
                isMuted = AudioManager.Instance.IsUIEffectsMuted;
                break;

            case AudioChannel.SFX:
                isMuted = AudioManager.Instance.IsSFXMuted;
                break;

            default:
                Debug.LogError("Invalid audio channel!");
                break;
        }

        unMuted.gameObject.SetActive(!isMuted);
        muted.gameObject.SetActive(isMuted);
    }

    public void ToggleState()
    {
        isMuted = !isMuted;

        switch (audioChannel)
        {
            case AudioChannel.Music:
                AudioManager.Instance.ToggleMusicMute();
                break;

            case AudioChannel.UI:
                AudioManager.Instance.ToggleUIEffectsMute();
                break;

            case AudioChannel.SFX:
                AudioManager.Instance.ToggleSFXMute();
                break;

            default:
                Debug.LogError("Invalid audio channel!");
                break;
        }

        unMuted.gameObject.SetActive(!isMuted);
        muted.gameObject.SetActive(isMuted);
    }

    public void SetVolume(float value)
    {
        switch (audioChannel)
        {
            case AudioChannel.Music:
                AudioManager.Instance.SetMusicVolume(value);
                break;

            case AudioChannel.UI:
                AudioManager.Instance.SetUIEffectsVolume(value);
                break;

            case AudioChannel.SFX:
                AudioManager.Instance.SetSFXVolume(value);
                break;

            default:
                Debug.LogError("Invalid audio channel!");
                break;
        }
    }
}
