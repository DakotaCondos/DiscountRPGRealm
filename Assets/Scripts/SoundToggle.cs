using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] UIBlock2D unMuted;
    [SerializeField] UIBlock2D muted;
    PersistentLink persistentLink;
    private bool isMuted = false;
    [SerializeField] AudioChannel audioChannel = AudioChannel.Music;

    private void Awake()
    {
        persistentLink = FindObjectOfType<PersistentLink>();
        if (persistentLink == null) Debug.LogWarning("SoundToggle could not find PersistentLink");
    }

    private void Start()
    {
        if (persistentLink == null) return;

        switch (audioChannel)
        {
            case AudioChannel.Music:
                isMuted = persistentLink.AudioManager.IsMusicMuted;
                break;

            case AudioChannel.UI:
                isMuted = persistentLink.AudioManager.IsUIEffectsMuted;
                break;

            case AudioChannel.SFX:
                isMuted = persistentLink.AudioManager.IsSFXMuted;
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
                persistentLink.AudioManager.ToggleMusicMute();
                break;

            case AudioChannel.UI:
                persistentLink.AudioManager.ToggleUIEffectsMute();
                break;

            case AudioChannel.SFX:
                persistentLink.AudioManager.ToggleSFXMute();
                break;

            default:
                Debug.LogError("Invalid audio channel!");
                break;
        }

        unMuted.gameObject.SetActive(!isMuted);
        muted.gameObject.SetActive(isMuted);
    }
}
