using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager; // 引用对话管理器
    [SerializeField] private string dialogueFilePath; // 对话文件路径

    private void OnMouseDown()
    {
        if (dialogueManager != null)
        {
            dialogueManager.LoadDialogueFile(dialogueFilePath); // 加载对话文件
            dialogueManager.StartDialogue(); // 开始对话
        }
        else
        {
            Debug.LogError("DialogueManager 未设置！");
        }
    }
}