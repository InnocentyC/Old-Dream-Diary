using UnityEngine;

public class RealityGameManager : MonoBehaviour
{
    public static RealityGameManager Instance;

    [Header("成年主角立绘")]
    public Sprite adult_neutral;        // MC_6 无表情
    public Sprite adult_tired;          // MC_1 疲惫
    public Sprite adult_confused;       // MC_2 疑惑
    public Sprite adult_angry;          // 生气
    public Sprite adult_surprised;      // 惊讶
    public Sprite adult_confused_hand; // 困惑（带手）

    // 获取AdultPortraitOption对应的Sprite
    public Sprite GetPortrait(AdultPortraitOption portraitOption)
    {
        switch (portraitOption)
        {
            case AdultPortraitOption.Adult_Neutral: return adult_neutral;
            case AdultPortraitOption.Adult_Tired: return adult_tired;
            case AdultPortraitOption.Adult_Confused: return adult_confused;
            case AdultPortraitOption.Adult_Angry: return adult_angry;
            case AdultPortraitOption.Adult_Surprised: return adult_surprised;
            case AdultPortraitOption.Adult_Confused_Hand: return adult_confused_hand;
            default: return null;
        }
    }

    // 获取SpeakerName（为了兼容新的DialogueLine）
    public string GetSpeakerName(SpeakerNameOption speakerOption)
    {
        return "雨漩"; // Script3中成年主控都叫"主控"
    }
    
    // Reality场景状态
    public enum RealityState
    {
        Intro,              // 开场
        Exploring,          // 自由探索阶段
        AllInteractionsDone,// 所有交互完成
        ReadyToSleep       // 准备睡觉，进入下阶段
    }

    public RealityState CurrentState { get; private set; }
    public bool IsUIBlocking { get; private set; }
    
    // 交互点完成状态
    private bool[] interactionCompleted;
    
    [Header("交互点数量")]
    public int totalInteractions = 5; // Bed, Computer, Notebook, FishDecoration, Medicine

    [Header("视频播放")]
    public GameObject videoPlayerObject;  // VideoPlayer 所在的游戏对象
    public UnityEngine.Video.VideoPlayer videoPlayer;  // VideoPlayer 组件
    public GameObject videoDisplayImage;   // 显示视频的 RawImage 对象

    [Header("童年记忆闪回")]
    public Sprite[] childhoodBackgrounds;  // 三张童年房间背景图
    public GameObject backgroundDisplay;   // 显示背景图的Image对象（RawImage或Image组件）
    public GameObject playerCharacter;      // 玩家角色游戏对象
    
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
        interactionCompleted = new bool[totalInteractions];
        StartRealityIntro();
    }
    
    private void StartRealityIntro()
    {
        EnterState(RealityState.Intro);

        // 开场对话
        var introDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="……做不完的工作，又是那么晚才下班。到底在坚持什么呢。"
                }
            }
        };

        // 确保DialogueManager存在
        if (DialogueManager.instance == null)
        {
            Debug.LogError("未找到DialogueManager！请在Scene3中添加DialogueManager GameObject");
            // 临时方案：直接进入探索模式
            EnterState(RealityState.Exploring);
            return;
        }

        DialogueManager.instance.StartDialogue(introDialogue, () => {
            EnterState(RealityState.Exploring);
        });
    }
    
    public void EnterState(RealityState newState)
    {
        CurrentState = newState;
        Debug.Log($"进入状态: {newState}");
        
        switch (newState)
        {
            case RealityState.Exploring:
                // 启用玩家移动和交互
                EnablePlayerControls(true);
                break;
                
            case RealityState.AllInteractionsDone:
                // 所有交互完成后的剧情
                TriggerAllDoneDialogue();
                break;
                
            case RealityState.ReadyToSleep:
                // 准备睡觉，床变得可交互
                Debug.Log("床现在可以交互了");
                break;
        }
        
        // 更新所有交互点的状态
        RefreshAllInteractables();
    }
    
    public void OnItemInteracted(RealityItemType type)
    {
        Debug.Log($"与 {type} 交互，当前状态: {CurrentState}");
        
        switch (CurrentState)
        {
            case RealityState.Exploring:
                HandleExploringInteraction(type);
                break;
                
            case RealityState.ReadyToSleep:
                HandleSleepInteraction(type);
                break;
        }
    }
    
    private void HandleExploringInteraction(RealityItemType type)
    {
        switch (type)
        {
            case RealityItemType.Bed:
                // 在Exploring阶段，床可以交互调查喵
                ShowBedEarlyInteraction();
                // 标记床的交互状态
                if (interactionCompleted[(int)RealityItemType.Bed] == false)
                {
                    interactionCompleted[(int)RealityItemType.Bed] = true;
                    Debug.Log($"床已标记为交互完成");
                }
                break;
                
            case RealityItemType.Computer:
                ShowComputerInteraction();
                HandleItemInteraction(type);
                break;
                
            case RealityItemType.Notebook:
                ShowNotebookInteraction();
                HandleItemInteraction(type);
                break;
                
            case RealityItemType.FishDecoration:
                ShowFishInteraction();
                HandleItemInteraction(type);
                break;
                
            case RealityItemType.Medicine:
                ShowMedicineInteraction();
                HandleItemInteraction(type);
                break;
        }
        
        CheckAllInteractionsComplete();
    }
    
    private void HandleSleepInteraction(RealityItemType type)
    {
        if (type == RealityItemType.Bed)
        {
            // 床的最终交互，进入下个场景
            ShowFinalBedInteraction();
        }
    }
    
    private void ShowBedEarlyInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Tired,
                    text="该睡觉了，但是丝毫不困。不过最近脑袋越来越重，好像忘记了什么一样。找找看有什么可以让我想起来关于过去的画面……"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    private void ShowComputerInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="看见电脑感觉我的价值只有工作。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="从前的自己好像从来没有这样想过。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="从那一刻开始，发生了这样可怕的变化。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="而我竟然也这样变得麻木。"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    private void ShowNotebookInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Confused_Hand,
                    text="一本本子，还是带密码锁的。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Confused_Hand,
                    text="尝试一下...想不起来密码了。"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    private void ShowFishInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Tired,
                    text="桌子上是热带鱼的全息摆件，总感觉在家里见过真的。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Tired,
                    text="现在要看热带鱼，只能去水族馆才能看见了吧。"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    private void ShowMedicineInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Tired,
                    text="要见底了，还是吃两颗吧。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Tired,
                    text="毕竟，明天早上还要起床。"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    private void ShowFinalBedInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Neutral,
                    text="可能是太累了，毕竟刚进入新城区工作，我还要成为新城区的公民。"
                }
            }
        };

        DialogueManager.instance.StartDialogue(dialogue, () => {
            // 转场 入睡动画 DH_001.MP4
            Debug.Log("播放入睡动画 DH_001.MP4");

            // 然后切换到下一个场景（Script4）
            UnityEngine.SceneManagement.SceneManager.LoadScene("Script4");
        });
    }
    
    private void HandleItemInteraction(RealityItemType type)
    {
        // 标记这个物品已经交互过
        int index = (int)type;
        if (index < interactionCompleted.Length)
        {
            interactionCompleted[index] = true;
        }
        
        Debug.Log($"物品 {type} 已标记为交互完成");
    }
    
    private void CheckAllInteractionsComplete()
    {
        int completedCount = 0;
        Debug.Log("=== 检查所有交互完成状态 ===");
        Debug.Log($"interactionCompleted.Length: {interactionCompleted.Length}");
        Debug.Log($"totalInteractions: {totalInteractions}");
        Debug.Log($"CurrentState: {CurrentState}");

        // 检查所有物品的交互状态
        for (int i = 0; i < interactionCompleted.Length && i < totalInteractions; i++)
        {
            Debug.Log($"索引 {i}: {(RealityItemType)i} - 已完成: {interactionCompleted[i]}");
            if (interactionCompleted[i])
            {
                completedCount++;
            }
        }

        Debug.Log($"已完成的交互: {completedCount}/{totalInteractions}");

        // 如果所有物品都交互完了
        if (completedCount >= totalInteractions && CurrentState == RealityState.Exploring)
        {
            Debug.Log("✓ 所有交互完成，触发下一步剧情");
            EnterState(RealityState.AllInteractionsDone);
        }
        else
        {
            Debug.Log($"✗ 条件不满足：completedCount >= totalInteractions? {completedCount >= totalInteractions}, CurrentState == Exploring? {CurrentState == RealityState.Exploring}");
        }
    }

    private void TriggerAllDoneDialogue()
    {
        Debug.Log("TriggerAllDoneDialogue 被调用");
        // 先播放花屏效果和童年视频
        StartCoroutine(PlayChildhoodMemorySequence());
    }

    private System.Collections.IEnumerator PlayChildhoodMemorySequence()
    {
        Debug.Log("PlayChildhoodMemorySequence 开始执行");

        // 确保之前的对话已经结束
        while (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive)
        {
            Debug.Log("等待上一场对话结束...");
            yield return null;
        }

        // 禁用玩家控制
        SetUIBlocking(true);

        // 等待0.5秒
        yield return new WaitForSeconds(0.5f);

        // 隐藏玩家角色（但保持在原位，相机跟随依然有效）
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(false);
            Debug.Log("玩家角色已隐藏");
        }
        else
        {
            Debug.LogWarning("未找到玩家角色游戏对象！");
        }

        // 显示背景图显示对象
        if (backgroundDisplay != null)
        {
            backgroundDisplay.SetActive(true);
            Debug.Log("背景图显示对象已激活");
        }
        else
        {
            Debug.LogWarning("未找到背景图显示对象！");
        }

        // 检查是否有三张童年背景图
        if (childhoodBackgrounds == null || childhoodBackgrounds.Length < 3)
        {
            Debug.LogError("童年背景图数量不足，需要至少3张！");
        }
        else
        {
            // 获取Image或RawImage组件
            var imageComponent = backgroundDisplay.GetComponent<UnityEngine.UI.Image>();
            var rawImageComponent = backgroundDisplay.GetComponent<UnityEngine.UI.RawImage>();

            // 每0.3秒切换一张背景图，共三张
            for (int i = 0; i < childhoodBackgrounds.Length; i++)
            {
                Debug.Log($"显示童年背景图 {i + 1}/3: {childhoodBackgrounds[i].name}");

                if (imageComponent != null)
                {
                    // 创建Sprite
                    imageComponent.sprite = childhoodBackgrounds[i];
                }
                else if (rawImageComponent != null)
                {
                    // RawImage需要使用Sprite
                    rawImageComponent.texture = childhoodBackgrounds[i].texture;
                }

                yield return new WaitForSeconds(0.3f);
            }
        }

        // 最后一张显示0.3秒后，隐藏背景图显示对象，恢复玩家角色可见
        yield return new WaitForSeconds(0.3f);

        // 隐藏背景图显示对象
        if (backgroundDisplay != null)
        {
            backgroundDisplay.SetActive(false);
            Debug.Log("背景图显示对象已隐藏");
        }

        // 恢复玩家角色可见
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(true);
            Debug.Log("玩家角色已恢复可见");
        }

        // 启用玩家控制
        SetUIBlocking(false);

        // 等待一帧，确保状态更新
        yield return null;

        // 显示幻觉后的对话
        var hallucinationDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Confused,
                    text="刚刚，好像出现了幻觉。"
                },
                new DialogueLine{
                    speaker=SpeakerNameOption.MainController,
                    adultPortrait=AdultPortraitOption.Adult_Confused,
                    text="算了，一定是太累了。先睡一觉吧。"
                }
            }
        };

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(hallucinationDialogue, () => {
                // 进入可以睡觉的状态
                EnterState(RealityState.ReadyToSleep);
            });
        }
        else
        {
            Debug.LogError("未找到 DialogueManager！");
            EnterState(RealityState.ReadyToSleep);
        }
    }
    
    private void RefreshAllInteractables()
    {
        // 刷新场景中所有可交互物品的状态
        RealityInteractableItem[] interactables = FindObjectsOfType<RealityInteractableItem>();
        foreach (var item in interactables)
        {
            item.RefreshInteractable();
        }
    }
    
    private void EnablePlayerControls(bool enable)
    {
        // 启用/禁用玩家控制
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = enable;
        }
    }
    
    public void SetUIBlocking(bool block)
    {
        IsUIBlocking = block;
        EnablePlayerControls(!block);
    }
}