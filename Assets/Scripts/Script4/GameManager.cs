using UnityEngine;


public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public string Name;
    //房间状态机
    public enum RoomState
    {
        Intro,              // 开场对白中
        NoteLocked,         // 未查看便利贴（密码物件锁定）
        PasswordCollecting, // 已查看便利贴，可收集密码
        AllTasksDone,       // 密码+日记完成
        ReadyToExit         // 点击床后，准备切场
    }
    public RoomState CurrentState { get; private set; }
    //UI阻断其他交互
    public bool IsUIBlocking { get; private set; }

    public void SetUIBlocking(bool block)
    {
        IsUIBlocking = block;
    }

    public DialogueManager dialogueManager;
    public SceneTransitionManager sceneTransitionManager;
    public TaskManager taskManager;

    public Sprite child_neutral,    // 幼年主控立绘 无表情
                  child_happy,      // 幼年主控立绘 开心
                  child_confused,   // 幼年主控立绘 疑惑
                  child_sad,        // 幼年主控立绘 伤心 / 失落
                  child_surprised,  // 幼年主控立绘 惊讶
                  child_pout;       // 幼年主控立绘 不满 / 生气 / 撅嘴 / 撇嘴


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 播放坐起动画
        // indObjectOfType<PlayerAnimationController>().PlaySitUpAnimation();

        // 开场对白

        EnterState(RoomState.Intro);
        StartIntroDialogue();
    }

    #region ===== State Control =====
    public void EnterState(RoomState newState)
    {
        CurrentState = newState;
        Debug.Log($"[RoomState] -> {newState}");

        foreach (var item in FindObjectsOfType<InteractableItem>())
        {
            item.SendMessage("RefreshInteractable", SendMessageOptions.DontRequireReceiver);
        }

        if (newState == RoomState.NoteLocked)
        {
            taskManager.ShowTaskUI();
        }

    }

    #endregion
    #region ===== Intro =====

    private void StartIntroDialogue()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ text="又是新的一天开始了。" },
                new DialogueLine{ text="或许连你自己还没有意识到，这一天多么不同。" },
                new DialogueLine{ speakerName=Name, text="这是哪？是我小时候的房间？", portrait=child_confused },
                new DialogueLine{ speakerName=Name, text="…我怎么变得这么矮……我变小了？！", portrait=child_confused },
                new DialogueLine{ text="这里是，旧城区，你的家。" }

            }
        };

        dialogueManager.StartDialogue(dialogue, () =>
        {
            EnterState(RoomState.NoteLocked);
        });
    }

    #endregion

    #region ===== Interaction Entry =====

    public void OnItemInteracted(ItemType type)
    {
        switch (CurrentState)
        {
            case RoomState.Intro:
                // 完全冻结输入
                return;

            case RoomState.NoteLocked:
                HandleNoteLocked(type);
                break;

            case RoomState.PasswordCollecting:
                HandlePasswordCollecting(type);
                break;

            case RoomState.AllTasksDone:
                HandleAllTasksDone(type);
                break;

            case RoomState.ReadyToExit:
                // 终态，不再响应
                return;
        }
    }

    #endregion



    #region ===== State Handlers =====

    private void HandleNoteLocked(ItemType type)
    {
        if (type == ItemType.Bed)
        {
            dialogueManager.PlayOneLine("好像还有什么东西没看完……");
            return;
        }
        if (type == ItemType.Note)
        {
            if (!taskManager.IsNoteViewed())
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
            dialogueManager.PlayOneLine("好像还有什么东西没看完……");
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
            EnterState(RoomState.AllTasksDone);
        }
    }

    private void HandleAllTasksDone(ItemType type)//交互结束
    {
       

        if (type == ItemType.Bed)
        {
            EnterState(RoomState.ReadyToExit);
            PlayExit();
        }
    }

    #endregion
    #region ===== Exit =====

    private void PlayExit()
    {
        sceneTransitionManager.SetNextScene("Script5");
        sceneTransitionManager.TransitionToNextScene();
    }

    #endregion

}
