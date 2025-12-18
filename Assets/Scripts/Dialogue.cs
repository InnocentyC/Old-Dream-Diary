using UnityEngine;
using TMPro; // 引用 TextMeshPro 命名空间

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // 单例，方便其他脚本调用

    public GameObject dialoguePanel; // 对话框 UI 物体
    public TextMeshProUGUI dialogueText; // 显示文字的组件

    void Awake()
    {
        instance = this;
        dialoguePanel.SetActive(false); // 游戏开始时隐藏对话框
    }

    // 显示对话
    public void ShowDialogue(string content)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = content;
        // 这里以后可以扩展打字机效果
    }

    // 关闭对话
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
