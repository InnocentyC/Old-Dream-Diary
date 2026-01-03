using UnityEngine;
using System.Collections;

public abstract class StateAction : ScriptableObject
{

    protected IGameManager GetManager()
    {
        // 1. 查找所有挂载了脚本的物体
        var allScripts = FindObjectsOfType<MonoBehaviour>();

        // 2. 遍历并找到第一个实现了 IGameManager 接口的组件
        foreach (var script in allScripts)
        {
            if (script is IGameManager manager)
            {
                return manager;
            }
        }

        Debug.LogError($"[StateAction] 找不到实现了 IGameManager 接口的组件！请确保当前场景（S4/S6）中有 GameManager 且已实现接口。");
        return null;
    }
    [Header("在哪个房间状态下触发")]
    public RoomState triggerState;   
    public abstract IEnumerator Execute();

}
