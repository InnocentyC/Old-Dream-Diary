using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//状态变了 → 顺序执行 Action

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
   
        public List<StateAction> stateActions;

        public IEnumerator ChangeState(GameManager.RoomState newState)
        {
            // GameManager.Instance.currentState = newState;

        // 执行所有匹配的 StateAction
            foreach (var action in stateActions)
            {
                if (action.triggerState == newState)
                {
                    yield return StartCoroutine(action.Execute());
                }
            }
        }
    
}
