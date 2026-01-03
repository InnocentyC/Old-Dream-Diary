using System.Collections;
using UnityEngine;
//Intro 对话后 → NoteLocked
//AllTasksDone → ReadyToExit

[CreateAssetMenu(fileName = "ChangRoomState",menuName = "Game/State Actions/Change Room State")]
public class ChangeRoomStateAction : StateAction
{
    [Header("输入状态枚举的名字 (例如: Intro, AllTasksDone)")]
    public RoomState nextState;
    public override IEnumerator Execute()
    {
        var manager = GetManager();
        if (manager != null)
        {
            Debug.Log($"<color=magenta>[Action]</color> ChangeRoomStateAction 开始执行 -> 目标: {nextState}");
            manager.EnterState(nextState);
            Debug.Log($"<color=magenta>[Action]</color> 状态已切换为: {manager.CurrentState}");
        }
        else
        {
            Debug.LogError($"<color=red>[Action Error]</color> ChangeRoomStateAction 找不到 GameManager!");
        }
        yield return null;
    }
}
