using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TaskModule
{
    void UpdateUI();         // 更新当前场景的 UI
    bool IsAllCompleted();   // 检查当前场景任务是否完成
}
