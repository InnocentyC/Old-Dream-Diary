using UnityEngine;
using System.Collections.Generic;    // 修复 Stack<> 报错
using System.Reflection;             // 修复 BindingFlags 和 FieldInfo 报错
using System;                        // 修复 Type 报错

public class TestStateController : MonoBehaviour
{
    [Header("测试快捷键说明 (大键盘数字键)")]
    [Header("1: Intro | 2: NoteLocked | 3: Password | 4: AllTasksDone | 5: Exit")]
    public bool enableShortcuts = true;

    private void Update()
    {
        if (!enableShortcuts) return;

        // 数字键 1: 重置为 Intro (开场)
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Switch(GameManager.RoomState.Intro, false);

        // 数字键 2: 到 NoteLocked (便利贴锁定)
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Switch(GameManager.RoomState.NoteLocked, false);

        // 数字键 3: 到 PasswordCollecting (可搜集密码)
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Switch(GameManager.RoomState.PasswordCollecting, false);

        // 数字键 4: 到 AllTasksDone (任务全满，可点床)
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Switch(GameManager.RoomState.AllTasksDone, true);

        // 数字键 5: 到 ReadyToExit (准备切场)
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Switch(GameManager.RoomState.ReadyToExit, true);
    }

    private void Switch(GameManager.RoomState target, bool fillTasks)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance 尚未初始化！");
            return;
        }

        Debug.Log($"<color=cyan>[Test Skip]</color> 触发键盘跳转 -> 目标状态: <color=yellow>{target}</color>");

        // 1. 暴力清理 UI 阻塞 (解决卡死的核心)
        GameManager.Instance.uiBlockCount = 0;

        // 2. 清理 Stack（反射）
        var stackField = typeof(GameManager).GetField("uiBlockStack", BindingFlags.NonPublic | BindingFlags.Instance);
        if (stackField != null)
        {
            var stack = (Stack<string>)stackField.GetValue(GameManager.Instance);
            stack?.Clear();
        }

        // 2. 强行终止并隐藏对话框
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StopAllCoroutines();
            DialogueManager.instance.dialoguePanel.SetActive(false);

            // 使用反射强制重置私有变量 isDialogueActive，防止 GameManager 逻辑被跳过
            var diagActiveField = typeof(DialogueManager).GetField("isDialogueActive", BindingFlags.NonPublic | BindingFlags.Instance);
            if (diagActiveField != null) diagActiveField.SetValue(DialogueManager.instance, false);
        }

        // 3. 补全或清空任务数据 (修改内存，不走物件交互)
        SyncTaskData(fillTasks);

        // 4. 执行状态跳转并刷新场景物件
        GameManager.Instance.EnterState(target);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(GameManager.Instance);
        if (GameManager.Instance.taskManager != null)
            UnityEditor.EditorUtility.SetDirty(GameManager.Instance.taskManager);
#endif
    }

    private void SyncTaskData(bool complete)
    {
        TaskManager tm = GameManager.Instance.taskManager;
        if (tm == null) return;

        // 使用反射强行修改 TaskManager 里的私有布尔变量名
        // 我记得你的代码里是这四个变量：fishCollected, dollCollected, awardCollected, diaryCollected
        SetPrivateBool(tm, "fishCollected", complete);
        SetPrivateBool(tm, "dollCollected", complete);
        SetPrivateBool(tm, "awardCollected", complete);
        SetPrivateBool(tm, "diaryCollected", complete);

        // 强制刷新 Task UI 界面显示
        tm.ShowTaskUI();
    }

    private void SetPrivateBool(object target, string fieldName, bool value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(target, value);
        }
        else
        {
            Debug.LogWarning($"未能在 TaskManager 中找到私有字段: {fieldName}");
        }
    }

}