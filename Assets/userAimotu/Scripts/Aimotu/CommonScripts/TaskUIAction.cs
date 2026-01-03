using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskUIAction", menuName = "Game/State Actions/TaskUI")]

public class TaskUIAction : StateAction
{
    public enum DreamStage { Dream1, Dream2 }
    [Header("属于哪个梦境阶段?")]
    public DreamStage stage;
    [Header("梦境1设置")]
    public ItemType collectType;
    public bool isDiary = false;
    [Header("梦境2设置")]
    public int diaryIndexD2; // 0-5
    public string diaryNameD2; // 例如输入 "2-小吃摊"

    [Header("音效配置")]
    public AudioClip unlockSound;      // UI_Diary_Unlock
    public bool onlyShowUI = false;


    public override IEnumerator Execute()
    {
        // 获取当前的 GameManager (通过接口)
        IGameManager gm = null;
        var allManagers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mono in allManagers)
        {
            if (mono is IGameManager managerInterface)
            {
                gm = managerInterface;
                break;
            }
        }
        if (gm != null && gm.TaskS4 != null)
        {
            // 直接操作拖进去的那个物体
            gm.TaskS4.gameObject.SetActive(true);

            if (onlyShowUI)
            {
                gm.TaskS4.ShowTaskUI();
                Debug.Log("<color=cyan>[Action]</color> TaskUI 已通过 GM 引用激活");
            }
            else
            {
                if (isDiary) gm.TaskS4.diaryCollected = true;
                else gm.TaskS4.CollectPassword(collectType);

                gm.TaskS4.UpdateUI();
            }
        }
        else
        {
            Debug.LogError("TaskUIAction 错误：GameManager 中未拖入 Task_S4 引用！");
        }
        yield break;
    }

}

