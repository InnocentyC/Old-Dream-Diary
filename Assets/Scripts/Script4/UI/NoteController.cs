using UnityEngine;

public class NoteController : MonoBehaviour
{
    public static NoteController Instance;

    [Header("UI References")]
    public NoteUI noteUI;
    public DialogueSession stickyNoteDialogue;

    private bool firstTimeHintCompleted = false; // 第一次 hint 完成标记

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

  
    //从 Notebook 点击小便利贴后调用
  
    public void Begin()
    {
        Debug.Log("[StickyNoteFlow] Begin");

        if (noteUI == null || stickyNoteDialogue == null)
        {
            Debug.LogError("[StickyNoteFlow] noteUI or stickyNoteDialogue is NULL!");
            return;
        }

        // ① 打开便利贴大图
        noteUI.Open();

        // ② 播放对话（注册结束回调）
        DialogueManager.instance.StartDialogue(
            stickyNoteDialogue,
            OnDialogueFinished
        );
    }

    // 对话结束后的统一收尾

    private void OnDialogueFinished()
    {
        Debug.Log("[StickyNoteFlow] Dialogue finished");

        // ③ 关闭便利贴大图
        noteUI.Close();

        // ④ 推进游戏
        GameManager.Instance.OnItemInteracted(ItemType.Note,null);
        if (!firstTimeHintCompleted)
        {
            GameManager.Instance.EnterState(GameManager.RoomState.PasswordCollecting);
            firstTimeHintCompleted = true;
        }
    }

}
