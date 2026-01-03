using UnityEngine;
using System.Collections.Generic;    // 修复 Stack<> 报错
using System.Reflection;             // 修复 BindingFlags 和 FieldInfo 报错
using System;                        // 修复 Type 报错
using S4;

    public class S4StateController : MonoBehaviour
    {
        [Header("S4 调试快捷键 (大键盘数字键)")]        
        [Header("1: Intro | 2: NoteLocked | 3: Password | 4: AllTasksDone | 5: Exit")]
        public bool enableShortcuts = true;

        private void Update()
        {
            if (!enableShortcuts) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) Switch(RoomState.Intro, false);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Switch(RoomState.NoteLocked, false);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Switch(RoomState.PasswordCollecting, false);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Switch(RoomState.AllTasksDone, true);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Switch(RoomState.ReadyToExit, true);
    }

        private void Switch(RoomState target, bool fillTasks)
        {
            var gm = S4.GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("S4.GameManager.Instance 尚未初始化！");
                return;
            }

            Debug.Log($"<color=cyan>[Test Skip]</color> 触发键盘跳转 -> 目标状态: <color=yellow>{target}</color>");

            // 1. 暴力清理 UI 阻塞 (解决卡死的核心)
            gm.uiBlockCount = 0;

            // 2. 清理 Stack（反射）
            var stackField = typeof(S4.GameManager).GetField("uiBlockStack", BindingFlags.NonPublic | BindingFlags.Instance);
            if (stackField != null)
            {
                var stack = (Stack<string>)stackField.GetValue(gm);
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
            gm.EnterState(target);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gm);
            var tm = FindObjectOfType<S4.Task_S4>();
            if (tm != null)
                UnityEditor.EditorUtility.SetDirty(tm);
#endif
        }

        private void SyncTaskData(bool complete)
        {
            var tm = FindObjectOfType<S4.Task_S4>();
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
