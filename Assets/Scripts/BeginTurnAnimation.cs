using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnAnimation : MonoBehaviour
{
    [SerializeField] private UIBlock2D _backingBlock;
    [SerializeField] private UIBlock2D _playerblock;
    [SerializeField] private TextBlock _nameBlock;
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Animation _animation;
    public float AnimationTimeSeconds { get; private set; }

    private void Awake()
    {
        AnimationTimeSeconds = _animationClip.length;
    }

    public void BuildUI(Player player)
    {
        _backingBlock.Color = player.playerColor;
        _playerblock.Border.Color = player.playerColor;
        _playerblock.SetImage(player.playerTexture);
        _nameBlock.Text = player.PlayerName;
    }

    public void PlayAnimation()
    {
        _animation.Play();
    }

    public void StopAnimation()
    {
        _animation.Stop();
    }
}
