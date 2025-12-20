using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    [Header("说话人名字")]
    public string speakerName;

    [Header("当前立绘")]
    public Sprite portrait;    // 每一句都可以换表情

    [Header("对话内容")]
    [TextArea(3, 10)]
    public string text;
}

[System.Serializable]
public class DialogueSession
{
    public DialogueLine[] lines; // 对话数组

    private int currentLineIndex = 0; // 当前句子索引
    public UnityEvent OnDialogueFinished = new UnityEvent(); // 对话结束事件

    // 获取当前句子
    public DialogueLine GetCurrentSentence()
    {
        if (currentLineIndex < lines.Length)
        {
            return lines[currentLineIndex];
        }
        return null;
    }

    // 切换到下一句
    public void NextSentence()
    {
        if (currentLineIndex < lines.Length - 1)
        {
            currentLineIndex++;
        }
        else
        {
            // 触发对话结束事件
            OnDialogueFinished.Invoke();
        }
    }

    // 重置对话
    public void ResetDialogue()
    {
        currentLineIndex = 0;
    }
}
