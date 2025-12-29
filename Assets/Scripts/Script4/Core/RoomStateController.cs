using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//×´Ì¬±äÁË ¡ú Ë³ÐòÖ´ÐÐ Action

public class RoomStateController : MonoBehaviour
{
    public RoomStateEvent[] stateEvents;

    private void OnEnable()
    {
        GameManager.OnRoomStateChanged += HandleState;
    }

    private void OnDisable()
    {
        GameManager.OnRoomStateChanged -= HandleState;
    }

    private void HandleState(GameManager.RoomState state)
    {
        foreach (var e in stateEvents)
        {
            if (e.triggerState == state)
            {
                StartCoroutine(Execute(e.actions));
                break;
            }
        }
    }

    private IEnumerator Execute(List<StateAction> actions)
    {
        if (actions == null) yield break;

        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute();
        }
    }
   
}
