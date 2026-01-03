using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskUIAction", menuName = "Game/State Actions/TaskUI")]

public class TaskUIAction : StateAction
{
    public ItemType collectType;
    public bool isDiary = false;
    public bool onlyShowUI = false;

    [Header("音效配置")]
    public AudioClip unlockSound;      // UI_Diary_Unlock
   // public AudioClip interactionSound; // UI_Interaction_Confirm
    public override IEnumerator Execute()
    {
        /* while (GameManager.Instance.IsUIBlocking)
             yield return null;
         GameManager.Instance.taskManager.ShowTaskUI();
         yield break;*/
        // 1. 如果只是为了显示 UI（比如 Intro 结束时）

        if (onlyShowUI)
        {
            GameManager.Instance.taskManager.ShowTaskUI();
            yield break; // 直接退出，不走下面的加分逻辑
        }
        else
        {
            //AudioManager.Instance.PlaySFX(interactionSound);
            // 调用 TaskUI 更新

            if (isDiary)
            {
                bool alreadyCollected = GameManager.Instance.taskManager.IsDiaryCollected;
                GameManager.Instance.taskManager.CollectDiary();
                // 2. 播放日记专属解锁音
                if (!alreadyCollected && unlockSound != null)AudioManager.Instance.PlaySFX(unlockSound);
            }
            else GameManager.Instance.taskManager.CollectPassword(collectType);



            // 检查是否全集齐了，集齐了就自动切换房间状态
            if (GameManager.Instance.taskManager.AreAllTasksCompleted())
            {
                GameManager.Instance.EnterState(GameManager.RoomState.AllTasksDone);
            }
            yield break;
        }
    }
}

