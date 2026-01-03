using UnityEngine;
public enum SpeakerNameOption
{
    None,
    MainController,
}
public enum PortraitOption
{
    None,
    Child_Neutral,  // 幼年主控 无表情
    Child_Happy1,   // 开心1
    Child_Happy2,   // 开心2
    Child_Confused, // 疑惑
    Child_Surprised,// 惊讶
    Child_Pout      // 不满/撅嘴
}

[System.Serializable]
public class DialogueLine
{
    [Header("说话人名字")]
    public SpeakerNameOption speaker;   // 使用下拉枚举

    [Header("当前立绘")]
    public PortraitOption portrait;    // 每一句都可以换表情

    [Header("对话内容")]
    [TextArea(3, 10)]
    public string text;
   

    public Sprite GetPortrait(IGameManager gm)
    {
        if (gm == null) return null; // 【必须添加这行检查】防止报错
        return gm.GetCharacterPortrait(portrait);
    }
    public string GetSpeakerName()
    {
        switch (speaker)
        {
            case SpeakerNameOption.MainController: return "雨漩";
            default: return "";
        }
    }

}

[System.Serializable]
public class DialogueSession
{
    public DialogueLine[] lines; // 对话数组
}
