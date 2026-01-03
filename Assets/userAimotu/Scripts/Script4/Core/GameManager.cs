using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;        
using UnityEngine.Video;     


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string Name;
    public static event Action<RoomState> OnRoomStateChanged;
    public PlayClipAction playClipAction;
    public RawImage uiRawImage;
    public VideoPlayer uiVideoPlayer;
    public RenderTexture uiRenderTexture;
    public CanvasGroup transitionMaskGroup; // 在 Inspector 里拖入一个覆盖全屏的黑色 Image 的 CanvasGroup
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
        Debug.Log($"Screen Size: {Screen.width} x {Screen.height}, Aspect: {(float)Screen.width / Screen.height}");

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
        if (DialogueManager.instance.IsDialogueActive)
            return; // 当前对话在进行中，延迟状态事件
        foreach (var ev in roomStateEvents)
        {
            if (ev.triggerState == state && ev.actions != null)
            {
                                 
                StartCoroutine(ExecuteActionsSequentially(ev.actions));
             
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
        if (newState == RoomState.AllTasksDone || newState == RoomState.PasswordCollecting)
        {
            // 只有在确认 UI 真的关闭时才清理
            if (dialogueManager != null && !dialogueManager.dialoguePanel.activeInHierarchy)
            {
                uiBlockCount = 0;
                uiBlockStack.Clear();
            }
        }
        Debug.Log($"[RoomState] -> {newState}");
        OnRoomStateChanged?.Invoke(newState);
        foreach (var item in FindObjectsOfType<InteractableItem>())
        {
            item.RefreshInteractable();
        }
        TryPlayStateEvent(newState);

    }
 
    #endregion

}
