using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PopupAction",menuName = "Game/State Actions/Popup Info")]
public class PopupAction : StateAction
{
    public enum PopupMode { Open, Close }
    [Header("模式切换")]
    public PopupMode mode;

    [Header("仅在 Open 模式下配置")]
    [TextArea] public string content;
    public StickerType sticker;
    public override IEnumerator Execute()
    {
        bool isClosed = false;
        if (mode == PopupMode.Open)
        {
            // 执行打开逻辑
            PopupSystem.Instance.Open(content, sticker, () => {
                isClosed = true;
            });
        }
        else
        {
            // 执行关闭逻辑
            PopupSystem.Instance.Close();
        }
        // 统一不阻塞序列，让对话能同时出现
        yield break;
    }
}