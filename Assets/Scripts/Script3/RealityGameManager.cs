using UnityEngine;

public class RealityGameManager : MonoBehaviour
{
    public static RealityGameManager Instance;
    
    [Header("成年主角立绘")]
    public Sprite adult_neutral;    // MC_6 无表情
    public Sprite adult_tired;       // MC_1 疲惫
    public Sprite adult_confused2;   // MC_2 疑惑
    public Sprite adult_confused3;   // MC_3 疑惑
    public Sprite adult_angry;        // 生气
    public Sprite adult_surprised;   // 惊讶
    public Sprite adult_confused_hand;    // 困惑（带手）
    
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
                    text="……做不完的工作，又是那么晚才下班。到底在坚持什么呢。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
                }
            }
        };
        
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(introDialogue, () => {
                EnterState(RealityState.Exploring);
            });
        }
        else
        {
            Debug.LogWarning("未找到DialogueManager，直接进入探索模式");
            EnterState(RealityState.Exploring);
        }
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
                    text="该睡觉了，但是丝毫不困。不过最近脑袋越来越重，好像忘记了什么一样。找找看有什么可以让我想起来关于过去的画面……",
                    speakerName="主控",
                    portrait=adult_tired // MC_1
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
                    text="看见电脑感觉我的价值只有工作。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
                },
                new DialogueLine{ 
                    text="从前的自己好像从来没有这样想过。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
                },
                new DialogueLine{ 
                    text="从那一刻开始，发生了这样可怕的变化。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
                },
                new DialogueLine{ 
                    text="而我竟然也这样变得麻木。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
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
                    text="一本本子，还是带密码锁的。",
                    speakerName="主控",
                    portrait=adult_confused3 // MC_3
                },
                new DialogueLine{ 
                    text="尝试一下...想不起来密码了。",
                    speakerName="主控",
                    portrait=adult_confused3 // MC_3
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
                    text="桌子上是热带鱼的全息摆件，总感觉在家里见过真的。",
                    speakerName="主控",
                    portrait=adult_tired // MC_1
                },
                new DialogueLine{ 
                    text="现在要看热带鱼，只能去水族馆才能看见了吧。",
                    speakerName="主控",
                    portrait=adult_tired // MC_1
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
                    text="要见底了，还是吃两颗吧。",
                    speakerName="主控",
                    portrait=adult_tired // MC_1
                },
                new DialogueLine{ 
                    text="毕竟，明天早上还要起床。",
                    speakerName="主控",
                    portrait=adult_tired // MC_1
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
                    text="可能是太累了，毕竟刚进入新城区工作，我还要成为新城区的公民。",
                    speakerName="主控",
                    portrait=adult_neutral // MC_6
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
        // 检查所有物品的交互状态
        for (int i = 0; i < interactionCompleted.Length && i < totalInteractions; i++)
        {
            if (interactionCompleted[i]) 
            {
                completedCount++;
                Debug.Log($"已完成的物品: {(RealityItemType)i}");
            }
        }
        
        Debug.Log($"已完成的交互: {completedCount}/{totalInteractions}");
        
        // 如果所有物品都交互完了
        if (completedCount >= totalInteractions && CurrentState == RealityState.Exploring)
        {
            Debug.Log("所有交互完成，触发下一步剧情");
            EnterState(RealityState.AllInteractionsDone);
        }
    }
    
    private void TriggerAllDoneDialogue()
    {
        // 先播放花屏效果和童年视频
        StartCoroutine(PlayChildhoodMemorySequence());
    }
    
    private System.Collections.IEnumerator PlayChildhoodMemorySequence()
    {
        // 画面短暂出现花屏，闪过童年房间
        Debug.Log("播放花屏效果");
        
        // 等待花屏效果
        yield return new WaitForSeconds(1.0f);
        
        // 播放童年房间动画 DH_002.MP4
        Debug.Log("播放童年房间动画 DH_002.MP4");
        
        // 等待视频播放完成
        yield return new WaitForSeconds(3.0f);
        
        // 显示幻觉后的对话
        var hallucinationDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="刚刚，好像出现了幻觉。",
                    speakerName="主控",
                    portrait=adult_confused2 // MC_2
                },
                new DialogueLine{ 
                    text="算了，一定是太累了。先睡一觉吧。",
                    speakerName="主控",
                    portrait=adult_confused2 // MC_2
                }
            }
        };
        
        DialogueManager.instance.StartDialogue(hallucinationDialogue, () => {
            // 进入可以睡觉的状态
            EnterState(RealityState.ReadyToSleep);
        });
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