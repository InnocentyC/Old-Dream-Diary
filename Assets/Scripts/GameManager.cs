using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DialogueManager dialogueManager;
    public SceneTransitionManager sceneTransitionManager;
    public TaskManager taskManager;
    public InteractableItem interactableItem;

    public Sprite Playerportrait1;//切换不同立绘表情
    private bool allInteractionsCompleted = false;

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
        StartDialogueSequence();
    }

    //开场对白----------------------------------------------------------------------
    private void StartDialogueSequence()
    {
        var openingDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{speakerName ="主控", text ="我小时候的房间？...我好像还变小了。",portrait = Playerportrait1 }
            }
        };

        dialogueManager.StartDialogue(openingDialogue, OnDialogueFinished);
    }
    private void OnDialogueFinished()
    {
        Debug.Log("对话结束！");
    }


    //交互逻辑------------------------------------------------------------------------------
    public void OnItemInteracted(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.NoteBook:
                // 找到密码本的一部分
                Debug.Log("找到密码本的一部分！");
                taskManager.FindPasswordPiece();
                break;

            case ItemType.Note:
                // 找到便利贴（假设这是日记的一部分）
                Debug.Log("找到便利贴！");
                taskManager.FindDiary();
                break;

            case ItemType.Bed:
                // 床可能没有任务，只显示对话或其他逻辑
                Debug.Log("床没有任务，只是触发了对话。");
                break;

            case ItemType.FishTank:
                // 特殊物品交互逻辑
                Debug.Log("触发了鱼缸的交互逻辑！");
                break;

            case ItemType.Doll:
                Debug.Log("触发了熊玩偶的交互逻辑！");
                break;

            case ItemType.Award:
                Debug.Log("触发了奖状的交互逻辑！");
                break;

            case ItemType.Beads:
                Debug.Log("触发了串珠的交互逻辑！");
                break;

            default:
                Debug.LogWarning($"未处理的物品类型: {itemType}");
                break;
        }

        // 检查任务是否完成
        CheckTasks();
    }
    private void CheckTasks()
    {
        if (taskManager.AreAllTasksCompleted())
        {
            Debug.Log("所有任务完成！准备切换到下一场景。");

            // 设置下一场景并触发场景切换
            sceneTransitionManager.SetNextScene("NextSceneName"); // 替换为你的场景名称
            sceneTransitionManager.TransitionToNextScene();
        }
    }

    public void CheckInteractions()
    {
        if (AllInteractionsCompleted())
        {
            allInteractionsCompleted = true;


            sceneTransitionManager.TransitionToNextScene();
        }
    }

    private bool AllInteractionsCompleted()
    {
        return taskManager.AreAllTasksCompleted();
    }
}
