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
        if (gm != null && gm.TaskModuleObject != null)
        {
            GameObject taskObj = gm.TaskModuleObject;
            // 直接操作拖进去的那个物体
            if (stage == DreamStage.Dream1)
            {
                var taskS4 = taskObj.GetComponent<S4.Task_S4>();
                if (taskS4 != null)
                {
                    if (onlyShowUI) taskS4.ShowTaskUI();
                    else
                    {
                        if (isDiary) taskS4.diaryCollected = true;
                        else taskS4.CollectPassword(collectType);

                        taskS4.UpdateUI();
                    }

                }
            }
            else if (stage == DreamStage.Dream2)
            {
                // 梦境 2：寻找 S6 脚本
                var taskS6 = taskObj.GetComponent<S6.Task_S6>();
                if (taskS6 != null)
                {
                    if (onlyShowUI) taskS6.ShowTaskUI();
                    else
                    {
                        // 使用我们在 Task_S6 里定义的数组逻辑
                        taskS6.CollectDiary(diaryIndexD2, diaryNameD2);
                    }
                }
            }
        }
        yield break;
    }

}

