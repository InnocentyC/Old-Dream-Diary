using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;        
using UnityEngine.Video;

namespace S4
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static GameManager Instance;
        public string Name;
        public static event Action<RoomState> OnRoomStateChanged;
        public PlayClipAction playClipAction;
        [Header("UI 引用绑定")]
        public Task_S4 taskS4_Instance; // 在 Unity Inspector 里把 Hierarchy 里的 TaskUI 拖到这里
     
        public GameObject TaskModuleObject => taskS4_Instance != null ? taskS4_Instance.gameObject : null;
        [Header("音效中心")]
        public AudioSource sfxSource;
        // 实现接口属性
        public S4.Task_S4 TaskS4
        {
            get { return taskS4_Instance; }
        }
        public DialogueManager Dialog => dialogueManager;

        [Header("Video Setup")]
        // Inspector 里拖入对应的组件
        [SerializeField] private RawImage _uiRawImage;
        [SerializeField] private VideoPlayer _uiVideoPlayer;
        [SerializeField] private RenderTexture _uiRenderTexture;
        [SerializeField] private CanvasGroup _transitionMaskGroup;        // 在 Inspector 里拖入一个覆盖全屏的黑色 Image 的 CanvasGroup
        public RawImage uiRawImage => _uiRawImage;
        public VideoPlayer uiVideoPlayer => _uiVideoPlayer;
        public RenderTexture uiRenderTexture => _uiRenderTexture;
        public CanvasGroup transitionMaskGroup => _transitionMaskGroup;
        // 在 Inspector 里拖入一个覆盖全屏的黑色 Image 的 CanvasGroup

        [System.Serializable]
        public class StateActionMapping
        {
            public RoomState state;
            public List<StateAction> actions;
        }
        public List<StateActionMapping> stateActionSettings;

        [HideInInspector] public Task_S4 task1;
        public Sprite GetCharacterPortrait(PortraitOption option)
        {
            switch (option)
            {
                case PortraitOption.Child_Neutral: return child_neutral;
                case PortraitOption.Child_Happy1: return child_happy1;
                case PortraitOption.Child_Happy2: return child_happy2;
                case PortraitOption.Child_Confused: return child_confused;
                case PortraitOption.Child_Surprised: return child_surprised;
                case PortraitOption.Child_Pout: return child_pout;
                default: return null;
            }
        }
    
        public RoomState CurrentState { get; private set; }

        //UI阻断其他交互
        [Header("UI Block Debug")]
        public int uiBlockCount = 0;
        private Stack<string> uiBlockStack = new Stack<string>();
        public bool IsUIBlocking => uiBlockCount > 0;

        public DialogueManager dialogueManager;
        //public SceneTransitionManager sceneTransitionManager;
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
                // 自动寻找挂在自己身上的插件
                task1 = GetComponent<Task_S4>();
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
        public void PlayGlobalSFX(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null || sfxSource == null) return;

            // PlayOneShot 的第二个参数就是音量比例 (0.0 到 1.0)
            sfxSource.PlayOneShot(clip, volume);
        }

    }
    public static class StateLogger
    {
        // 带有颜色标签的日志格式，方便在 Console 中快速定位
        public static void LogStateChange(RoomState oldState, RoomState newState, string source)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logMsg = $"<color=#4FC3F7>[State Trace]</color> [{timeStamp}]\n" +
                            $"<b>FROM:</b> <color=#FF8A65>{oldState}</color>\n" +
                            $"<b>TO:</b> <color=#81C784>{newState}</color>\n" +
                            $"<b>TRIGGER:</b> {source}";

            Debug.Log(logMsg);
        }

        public static void LogTaskProgress(string itemName, int current, int total)
        {
            Debug.Log($"<color=#DCE775>[Task Progress]</color> 收集了: <b>{itemName}</b> | 当前进度: <color=yellow>{current}/{total}</color>");
        }
    }

}