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
}
