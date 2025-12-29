using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Room State Event")]
public class RoomStateEvent : ScriptableObject
{
    public GameManager.RoomState triggerState;

    [Header("进入该状态时执行的行为")]
    public List<StateAction> actions;
}
