using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;
    public GameObject rootPanel;

    private void Awake()
    {
        Instance = this;
        rootPanel.SetActive(false);
    }

    public void Open()
    {
        rootPanel.SetActive(true);
        GameManager.Instance.PushUIBlock("Notebook");
    }

    public void Close()
    {
        rootPanel.SetActive(false);
        GameManager.Instance.PopUIBlock("Notebook");
    }

    public void OnStickyNoteClicked()
    {
        Debug.Log("STICKY NOTE BUTTON CLICKED");
        Close();
        if (NoteController.Instance != null)
            NoteController.Instance.Begin();
        else
            Debug.LogError("[NotebookUI] NoteController.Instance is null");
    }
}
