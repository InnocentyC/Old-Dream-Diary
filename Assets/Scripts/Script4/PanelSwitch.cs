using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : MonoBehaviour
{
    [Header("面板切换")]
    public PanelController closePanel;
    public PanelController openPanel;

    [Header("可选：流程通知")]
    public bool markNoteViewed;
    public bool enterPasswordState;

    public void Execute()
    {
        if (closePanel != null)
            closePanel.Hide();

        if (openPanel != null)
            openPanel.Show();

        if (markNoteViewed)
        {
            GameManager.Instance.taskManager.ViewNote();
        }

        if (enterPasswordState)
        {
            GameManager.Instance.EnterState(GameManager.RoomState.PasswordCollecting);
        }
    }
}
