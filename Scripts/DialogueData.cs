using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string[] Lines; // 对话内容数组

    public DialogueData(string[] lines)
    {
        Lines = lines;
    }
}
