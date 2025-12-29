using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public string Name;
    public static event Action<RoomState> OnRoomStateChanged;


    //房间状态机
    public enum RoomState
    {
        None,
        Intro,              // 开场对白中
        NoteLocked,         // 未查看便利贴（密码物件锁定）
        PasswordCollecting, // 已查看便利贴，可收集密码
        AllTasksDone,       // 密码+日记完成
        ReadyToExit         // 点击床后，准备切场
    }
    public RoomState CurrentState { get; private set; }

    //UI阻断其他交互
    [Header("UI Block Debug")]
    public int uiBlockCount = 0;
    private Stack<string> uiBlockStack = new Stack<string>();
    public bool IsUIBlocking => uiBlockCount > 0;

    public DialogueManager dialogueManager;
    //public SceneTransitionManager sceneTransitionManager;
    public TaskManager taskManager;
    public List<RoomStateEvent> roomStateEvents;

    public Sprite child_neutral,    // 幼年主控立绘 无表情
                  child_happy1,      // 幼年主控立绘 开心1
                  child_happy2,   // 幼年主控立绘 开心2
                  child_confused,        // 幼年主控立绘 疑惑
                  child_surprised,  // 幼年主控立绘 惊讶
                  child_pout;       // 幼年主控立绘 不满 / 生气 




    private void Awake()
        {
           if (Instance == null)
           {
               Instance = this;
               DontDestroyOnLoad(gameObject);
           }
           else Destroy(gameObject);
        }
    private void Start()
    {
        EnterState(RoomState.Intro);
    }

    public void PushUIBlock(string source = "Unknown")
    {
        uiBlockCount++;
        uiBlockStack.Push(source);
        Debug.Log($"[UIBlock] PUSH by {source} -> {uiBlockCount}");
    }
    public void PopUIBlock(string source = "Unknown")
    {
        if (uiBlockCount <= 0)
        {
            Debug.LogWarning($"[UIBlock] POP by {source} but count already 0");
            return;
        }

        uiBlockCount--;
        string last = uiBlockStack.Count > 0 ? uiBlockStack.Pop() : "Unknown";

        Debug.Log($"[UIBlock] POP by {source} (last: {last}) -> {uiBlockCount}");
    }
    /* private void OnGUI()
     {
         GUI.color = uiBlockCount > 0 ? Color.red : Color.green;
         GUI.Label(new Rect(10, 10, 400, 25),
             $"UIBlockCount: {uiBlockCount}");
     }*/



    #region ===== State Control =====
    private void TryPlayStateEvent(RoomState state)
    {
        foreach (var ev in roomStateEvents)
        {
            if (ev.triggerState == state && ev.actions != null)
            {
                foreach (var action in ev.actions)
                {                    
                    StartCoroutine(ExecuteActionsSequentially(ev.actions));
                }
            }
        }
    }
    private IEnumerator ExecuteActionsSequentially(List<StateAction> actions)
    {
        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute(); // 等待完成再执行下一个
        }
    }
    public void EnterState(RoomState newState)
    {
        CurrentState = newState;
        Debug.Log($"[RoomState] -> {newState}");
        OnRoomStateChanged?.Invoke(newState);
        foreach (var item in FindObjectsOfType<InteractableItem>())
        {
            item.RefreshInteractable();
        }
        TryPlayStateEvent(newState);

        if (newState == RoomState.NoteLocked)
        {
            taskManager.ShowTaskUI();
        }

    }

    #endregion
    

    #region ===== Interaction Entry =====

    public void OnItemInteracted(ItemType type, InteractableItem item)
    {
        if (IsUIBlocking) return;
        // 数据化执行：物体自身事件
        if (item != null && item.stateEvents != null)
        {
            foreach (var evt in item.stateEvents)
            {
                if (evt.triggerState == CurrentState)
                {
                    StartCoroutine(ExecuteActions(evt.actions));
                }
            }
        }
    
     switch (CurrentState)
     {
         case RoomState.NoteLocked:
             HandleNoteLocked(type);
             break;

         case RoomState.PasswordCollecting:
             HandlePasswordCollecting(type);
             break;

         case RoomState.AllTasksDone:
             HandleAllTasksDone(type);
             break;

     }
    }
    private IEnumerator ExecuteActions(List<StateAction> actions)
    {
        if (actions == null) yield break;

        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute();
        }
    }
    #endregion


    
        #region ===== State Handlers =====

       private void HandleNoteLocked(ItemType type)
        {

            if (type == ItemType.Note)
            {
                if (!taskManager.IsNoteViewed() && !taskManager.IsNoteViewed())
                {
                    taskManager.ViewNote();
                    EnterState(RoomState.PasswordCollecting);
                }
                return;
            }

            if (type == ItemType.Beads)
            {
                taskManager.CollectDiary();
                return;
            }

            Debug.Log("请先查看便利贴");
        }
        private void HandlePasswordCollecting(ItemType type)
        {
            if (type == ItemType.Bed)
            {
                //dialogueManager.PlayOneLine("好像还有什么东西没看完……");
                return;
            }
            if (type == ItemType.FishTank ||
                type == ItemType.Doll ||
                type == ItemType.Award)
            {
                taskManager.CollectPassword(type);
            }
            else if (type == ItemType.Beads)
            {
                taskManager.CollectDiary();
            }

            if (taskManager.AreAllTasksCompleted())
            {
                 StartCoroutine(WaitForDialogueThenAllTasksDone());
            }
        }
        private IEnumerator WaitForDialogueThenAllTasksDone()
        {
            // 等待当前 UIBlock 消失
            while (IsUIBlocking)
                yield return null;

            EnterState(RoomState.AllTasksDone);
        }
        private void HandleAllTasksDone(ItemType type)//交互结束
            {


                if (type == ItemType.Bed)
                {
                    EnterState(RoomState.ReadyToExit);
                   // PlayExit();
                }
            }

        #endregion
        #region ===== Exit =====

        private void PlayExit()
        {
            
        }

        #endregion
       
}
