using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicTracks = new();
    public int currentTrack = 0;

    public static MusicManager Instance { get; private set; }
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

    private void OnEnable() => AudioManager.MusicTrackEnded += HandleMusicTrackEnded;
    private void OnDisable() => AudioManager.MusicTrackEnded -= HandleMusicTrackEnded;

    private void Start()
    {
        if (musicTracks.Count > 0) { PlayNextTrack(); }
    }

    private void HandleMusicTrackEnded()
    {
        currentTrack = (currentTrack + 1 < musicTracks.Count) ? currentTrack + 1 : 0;
        PlayNextTrack();
    }

    private void PlayNextTrack()
    {
        AudioManager.Instance.PlaySound(musicTracks[currentTrack], AudioChannel.Music, false);
    }
}
