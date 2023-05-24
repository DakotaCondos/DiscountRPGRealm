using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelSwitcher : MonoBehaviour
{
    public UIBlock2D activePanel;
    public List<UIBlock2D> panels;

    private void Start()
    {
        // Set the first panel as active at the start
        if (panels.Count > 0)
        {
            SetActivePanel(panels[0]);
        }
    }

    public void SetActivePanel(UIBlock2D panel)
    {
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
    }

}
