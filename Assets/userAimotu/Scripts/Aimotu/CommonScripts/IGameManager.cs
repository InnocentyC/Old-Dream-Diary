using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using S4;
using S6;

public interface IGameManager
{
    // 定义通用代码需要调用的方法
    RoomState CurrentState { get; }
    void EnterState(RoomState newState);

    void PushUIBlock(string source);
    void PopUIBlock(string source);
    Sprite GetCharacterPortrait(PortraitOption option);
    DialogueManager Dialog { get; }        // 对话管理
    // 视频组件引用 (供 PlayClipAction 使用)
    RawImage uiRawImage { get; }
    VideoPlayer uiVideoPlayer { get; }
    RenderTexture uiRenderTexture { get; }
    CanvasGroup transitionMaskGroup { get; }
    bool IsUIBlocking { get; }

    Coroutine StartCoroutine(IEnumerator routine);    // 如果需要切换状态，可以加这个
    S4.Task_S4 TaskS4 { get; } // 新增：任务UI组件的引用
//S6.Task_S6 TaskS6 { get; } // 新增：任务UI组件的引用
    void PlayGlobalSFX(AudioClip clip);

}