using UnityEngine;
using System.Collections;
//可被 RoomStateController 调用的一段“状态进入行为”
public abstract class StateAction : ScriptableObject
{

    [Header("在哪个房间状态下触发")]
    public GameManager.RoomState triggerState;
    /// 当进入对应 RoomState 时执行
    public abstract IEnumerator Execute();

}
