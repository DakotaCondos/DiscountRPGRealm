using Nova;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationPanelManager : SceneSingleton<AnimationPanelManager>
{
    [SerializeField] private BeginTurnAnimation _beginTurnAnimation;
    [SerializeField] private PanelSwitcher _panelSwitcher;

    [Header("Panels")]
    [SerializeField] private UIBlock2D _beginTurnPanel;


    public async Task BeginTurnAnimation(TurnActor actor)
    {
        Player player = actor.player;
        _beginTurnAnimation.BuildUI(player);
        _panelSwitcher.SetActivePanel(_beginTurnPanel);
        _beginTurnAnimation.PlayAnimation();

        if (actor.isPlayer) { CameraController.Instance.ClearFocusObject(player.currentSpace.gameObject); }

        await Task.Delay(Mathf.RoundToInt(1000 * _beginTurnAnimation.AnimationTimeSeconds));
        _panelSwitcher.HideAll();
        _beginTurnAnimation.StopAnimation();
    }
}
