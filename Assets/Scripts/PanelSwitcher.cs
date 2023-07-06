using Nova;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public UIBlock2D activePanel;
    public List<UIBlock2D> panels;

    private void Start()
    {
        if (activePanel != null)
        {
            foreach (UIBlock2D p in panels)
            {
                if (p == activePanel)
                {
                    p.gameObject.SetActive(true);
                }
                else
                {
                    p.gameObject.SetActive(false);
                }
            }
            return;
        }

        // Set the first panel as active at the start
        if (panels.Count > 0)
        {
            SetActivePanel(panels[0]);
        }
    }

    public void SetActivePanel(UIBlock2D panel)
    {
        if (activePanel != null && panel.Equals(activePanel)) { return; }

        if (ApplicationManager.Instance.handlerNotificationsEnabled)
        {
            ConsolePrinter.PrintToConsole($"SetActivePanel({panel.name})", Color.white);
        }

        foreach (UIBlock2D p in panels)
        {
            if (p == panel)
            {
                p.gameObject.SetActive(true);
            }
            else
            {
                p.gameObject.SetActive(false);
            }
        }
        activePanel = panel;
    }

}
