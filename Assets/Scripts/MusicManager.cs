using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicTracks;
    public List<AudioClip> MainGameTracks = new();
    public List<AudioClip> EndGameTracks = new();
    public int currentTrack = 0;
    public int trackset = 0;

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
        musicTracks = MainGameTracks;
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

    public void MainGameMusic()
    {
        musicTracks = MainGameTracks;
        currentTrack = 0;
        PlayNextTrack();
    }

    public void EndGameMusic()
    {
        musicTracks = EndGameTracks;
        currentTrack = 0;
        PlayNextTrack();
    }
}
