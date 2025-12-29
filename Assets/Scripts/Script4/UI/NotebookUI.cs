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

   
    private bool stickyClickable = false;      // 是否可以点击便利贴
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
        GameManager.Instance.PushUIBlock("Notebook");

        if (!HasOpenedOnce && hintDialogue != null)
        {
            stickyClickable = false; // hint 播放前不可点击
            SetStickyHighlight(true); // 高亮提示
            StartCoroutine(PlayHintDialogue());
            HasOpenedOnce = true;
        }
        else
        {
            stickyClickable = true; // 第二次打开直接可以点击
            SetStickyHighlight(true); // 高亮提示

        }

    }

    public void Close()
    {
        rootPanel.SetActive(false);
        GameManager.Instance.PopUIBlock("Notebook");
        SetStickyHighlight(false);
    }
    private IEnumerator PlayHintDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        if (hintDialogue != null)
        {
            DialogueManager.instance.StartDialogue(
                hintDialogue,
                OnHintDialogueFinished
            );
        }
        else
        {
            // 没配对白也不阻断流程
            OnHintDialogueFinished();
        }
    }
    private void OnHintDialogueFinished()
    {
        // ③ 提示玩家：现在可以点便利贴了
        stickyClickable = true;
    }

    private void SetStickyHighlight(bool on)
    {
        if (stickyHighlight != null)
            stickyHighlight.SetActive(on);
    }
    public void OnStickyNoteClicked()
    {
        if (!stickyClickable)
        {
            Debug.Log("[NotebookUI] StickyNote click ignored, not ready yet");
            return;
        }
         Close();
        if (NoteController.Instance != null)
        {
            NoteController.Instance.Begin();

        }else
            Debug.LogError("[NotebookUI] NoteController.Instance is null");
    }

}
