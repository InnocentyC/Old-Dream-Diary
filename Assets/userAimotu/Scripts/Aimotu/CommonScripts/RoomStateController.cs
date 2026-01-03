using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class RoomStateController : MonoBehaviour
    {
        public RoomStateEvent[] stateEvents;
        private IGameManager GetManager() => FindObjectOfType<MonoBehaviour>() as IGameManager;
        private void OnEnable()
        {
            //GameManager.OnRoomStateChanged += HandleState;
        }

        private void OnDisable()
        {
           // GameManager.OnRoomStateChanged -= HandleState;
        }

        private void HandleState(RoomState state)
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

        public IEnumerator ChangeState(RoomState newState)
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
