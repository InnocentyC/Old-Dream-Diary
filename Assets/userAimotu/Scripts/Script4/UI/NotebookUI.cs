using System.Collections;
using UnityEngine;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;
    [Header("Root")]
    public GameObject rootPanel;

    [Header("Hint Dialogue")]
    public DialogueSession hintDialogue;  

    [Header("Sticky Note Highlight")]
    public GameObject stickyHighlight;     // 高亮物体（Animator）

   
    public bool HasOpenedOnce { get; private set; } = false;




    private void Awake()
    {
        Instance = this;
        rootPanel.SetActive(false);
        if (stickyHighlight != null)
            stickyHighlight.SetActive(false);
    }

    public void Open()
    {
        rootPanel.SetActive(true);
        //GameManager.Instance.PushUIBlock("Notebook");
        SetStickyHighlight(true); // 高亮提示
    }

    public void Close()
    {
        rootPanel.SetActive(false);
        //GameManager.Instance.PopUIBlock("Notebook");
        SetStickyHighlight(false);
    }


    private void SetStickyHighlight(bool on)
    {
        if (stickyHighlight != null)
            stickyHighlight.SetActive(on);
    }
  

}
