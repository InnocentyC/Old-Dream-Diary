using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequenceTrigger : MonoBehaviour
{
    [Header("点击后按顺序执行")]
    public List<StateAction> sequence;
    IGameManager GetManager() => FindObjectOfType<MonoBehaviour>() as IGameManager;
    public void TriggerSequence()
    {
        if (sequence == null || sequence.Count == 0)
        {
            // 【红灯 2】：如果这里打印了，说明你 Inspector 里的列表是空的
            Debug.LogWarning("Sequence 列表是空的！");
            return;
        }
        // 额外检查：防止连点
        // if (GameManager.Instance != null && GameManager.Instance.IsUIBlocking)
        Debug.Log("Button TriggerSequence Called!");
        var manager = GetManager();
        if (manager != null)
        {
            manager.StartCoroutine(ExecuteRoutine());
        }
        else
        {
            // 如果没找到接口，回退到自己的协程
            StartCoroutine(ExecuteRoutine());
        }
    }

    private IEnumerator ExecuteRoutine()
    {
        foreach (var action in sequence)
        {
            if(action != null)
            {
                    Debug.Log($"正在执行 Action: {action.name}");
                    yield return action.Execute();
            }
            else
            {
                    // 【红灯 3】：如果列表里有空槽位（Missing），也会卡住
                    Debug.LogError("发现空的 Action 槽位！");
            }
            
            Debug.Log("Action 序列执行完毕");
        }
    }
}
