using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Script5GameManager : MonoBehaviour
{
    public static Script5GameManager Instance;
    
    [Header("任务UI系统")]
    public GameObject taskUIPanel;
    public TextMeshProUGUI taskText1;  // 第一个任务文本
    public TextMeshProUGUI taskText2;  // 第二个任务文本
    
    [Header("密码本系统")]
    public GameObject diaryCoverUI;  // 密码本封面UI
    public GameObject diaryUI; // 日记本UI
    private DiaryUIManager diaryManager;
    
    [Header("对话系统")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public Image speakerPortrait;
    
    [Header("角色立绘")]
    public Sprite adult_confused3;   // MC_3 疑惑
    public Sprite adult_surprised;   // MC_5 惊讶
    public Sprite adult_neutral;    // MC_6 无表情
    public Sprite adult_tired;       // MC_1 疲惫
    
    [Header("游戏状态")]
    public bool isDiaryUnlocked = false;
    private bool[] interactionCompleted;
    
    [Header("交互点数量")]
    public int totalInteractions = 7; // PasswordNotebook, Computer, Window, Medicine, FishDecoration, Phone, Bed
    
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
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        // 初始化任务UI
        if (taskUIPanel != null)
            taskUIPanel.SetActive(false);
            
        // 初始化密码本封面UI
        if (diaryCoverUI != null)
            diaryCoverUI.SetActive(false);
            
        // 初始化日记本UI管理器
        diaryManager = FindObjectOfType<DiaryUIManager>();
        if (diaryManager == null)
        {
            Debug.LogWarning("未找到DiaryUIManager，请确保场景中有该组件");
        }
            
        // 开始游戏流程
        StartGameFlow();
    }
    
    private void StartGameFlow()
    {
        interactionCompleted = new bool[totalInteractions];
        StartCoroutine(GameFlowSequence());
    }
    
    private IEnumerator GameFlowSequence()
    {
        yield return new WaitForSeconds(1.0f);
        
        // 1. 显示两个任务UI
        ShowTaskUI("可以解锁密码本了", "解锁日记1-热带鱼和串珠");
        
        yield return new WaitForSeconds(3.0f);
        
        // 2. 隐藏任务UI
        HideTaskUI();
        
        yield return new WaitForSeconds(1.0f);
        
        // 3. 开始主角对话
        ShowIntroDialogue();
    }
    
    public void OnItemInteracted(Script5ItemType type)
    {
        Debug.Log($"与 {type} 交互");
        
        switch (type)
        {
            case Script5ItemType.PasswordNotebook:
                ShowPasswordNotebook();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.Computer:
                ShowComputerInteraction();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.Window:
                ShowWindowInteraction();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.Medicine:
                ShowMedicineInteraction();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.FishDecoration:
                ShowFishInteraction();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.Phone:
                ShowPhoneInteraction();
                HandleItemInteraction(type);
                break;
                
            case Script5ItemType.Bed:
                ShowBedInteraction();
                HandleItemInteraction(type);
                break;
        }
        
        CheckAllInteractionsComplete();
    }
    
    private void HandleItemInteraction(Script5ItemType type)
    {
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
        for (int i = 0; i < interactionCompleted.Length && i < totalInteractions; i++)
        {
            if (interactionCompleted[i]) 
            {
                completedCount++;
            }
        }
        
        Debug.Log($"已完成的交互: {completedCount}/{totalInteractions}");
    }
    
    public bool IsUIBlocked()
    {
        bool diaryActive = false;
        if (diaryManager != null)
            diaryActive = diaryManager.IsDiaryActive();
        else if (diaryUI != null)
            diaryActive = diaryUI.activeSelf;
            
        return (dialoguePanel != null && dialoguePanel.activeSelf) || 
               diaryActive ||
               (taskUIPanel != null && taskUIPanel.activeSelf) ||
               (diaryCoverUI != null && diaryCoverUI.activeSelf);
    }
    
    public bool IsDiaryUnlocked()
    {
        return isDiaryUnlocked;
    }
    
    public bool IsInteractionCompleted(Script5ItemType type)
    {
        int index = (int)type;
        return index < interactionCompleted.Length && interactionCompleted[index];
    }
    
    private void ShowComputerInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="Angela：「今天提交的报告，需要再优化一下哦。」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="刘总：「看到消息回一下。」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="工作助手：「您还有9条工作内容未上传，周末也要加油～」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="群聊：「收到。」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="群聊：「收到。」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="雨：「收到。」",
                    speakerName="电脑屏幕"
                },
                new DialogueLine{ 
                    text="......",
                    speakerName="主控",
                    portrait=adult_surprised
                },
                new DialogueLine{ 
                    text="……又来了。",
                    speakerName="主控",
                    portrait=adult_surprised
                },
                new DialogueLine{ 
                    text="烦人的工作。",
                    speakerName="主控",
                    portrait=adult_surprised
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    private void ShowWindowInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="天色已经那么暗了。",
                    speakerName="主控",
                    portrait=adult_neutral
                },
                new DialogueLine{ 
                    text="今天的时间好像从未属于我一样。",
                    speakerName="主控",
                    portrait=adult_neutral
                },
                new DialogueLine{ 
                    text="那种被留在旧世界的孤独突然造访。",
                    speakerName="主控",
                    portrait=adult_neutral
                },
                new DialogueLine{ 
                    text="在属于我一个人的房间里，不知道今晚还会不会做梦",
                    speakerName="主控",
                    portrait=adult_neutral
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    private void ShowMedicineInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="吃褪黑素好像吃出抗药性来了。……今天吃多少都睡不着吧。",
                    speakerName="主控",
                    portrait=adult_neutral
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    private void ShowFishInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="我真的写过日记这种东西吗....",
                    speakerName="主控",
                    portrait=adult_neutral
                },
                new DialogueLine{ 
                    text="这本日记是幻觉，还是我现在生活的是在梦境。",
                    speakerName="主控",
                    portrait=adult_neutral
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    private void ShowPhoneInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="妈妈：「未接来电」",
                    speakerName="手机"
                },
                new DialogueLine{ 
                    text="妈妈：「你已经很久没有和我们说过话了。」",
                    speakerName="手机"
                },
                new DialogueLine{ 
                    text="妈妈：「到底在忙什么呢?」",
                    speakerName="手机"
                },
                new DialogueLine{ 
                    text="健康监测：「警报！检测到使用者连续14天睡眠不足。」",
                    speakerName="手机"
                },
                new DialogueLine{ 
                    text="清理系统：「记忆清理项目更新：请及时配合。」",
                    speakerName="手机"
                },
                new DialogueLine{ 
                    text="......",
                    speakerName="主控",
                    portrait=adult_tired
                },
                new DialogueLine{ 
                    text="……好累。",
                    speakerName="主控",
                    portrait=adult_tired
                },
                new DialogueLine{ 
                    text="如果再睡一觉......还能看到那些..梦吗？",
                    speakerName="主控",
                    portrait=adult_tired
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    private void ShowBedInteraction()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="可能是太累了，毕竟刚进入新城区工作，我还要成为新城区的公民。",
                    speakerName="主控",
                    portrait=adult_neutral
                }
            }
        };
        
        ShowDialogue(dialogue, () => {
            // 转场 入睡动画 DH_001_MP4
            Debug.Log("播放入睡动画 DH_001_MP4");
            // 这里可以添加场景切换逻辑
        });
    }
    
    private void ShowIntroDialogue()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="刚刚那些是记忆？还是……梦？好真实......",
                    speakerName="主控",
                    portrait=adult_confused3 // MC_3
                },
                new DialogueLine{ 
                    text="好像在梦中得到了密码本的密码。",
                    speakerName="主控",
                    portrait=adult_confused3 // MC_3
                },
                new DialogueLine{ 
                    text="试试看吧。",
                    speakerName="主控",
                    portrait=adult_confused3 // MC_3
                }
            }
        };
        
        ShowDialogue(dialogue);
    }
    
    public void ShowTaskUI(string text1, string text2 = null)
    {
        if (taskUIPanel == null) return;
        
        if (this.taskText1 != null)
            this.taskText1.text = text1;
            
        if (this.taskText2 != null && text2 != null)
            this.taskText2.text = text2;
            
        taskUIPanel.SetActive(true);
        
        // 开始渐显效果 - 使用TaskUIPanel上的CanvasGroup
        CanvasGroup canvasGroup = taskUIPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            StartCoroutine(FadeTaskUI(canvasGroup, 0f, 1f, 0.25f)); // 弹出速度加倍：0.5f → 0.25f
        }
    }
    
    // 保持单文本兼容性的方法
    public void ShowSingleTaskUI(string taskText)
    {
        ShowTaskUI(taskText, null);
    }
    
    public void HideTaskUI()
    {
        if (taskUIPanel == null) return;
        
        // 开始渐隐效果 - 使用TaskUIPanel上的CanvasGroup
        CanvasGroup canvasGroup = taskUIPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            StartCoroutine(FadeTaskUI(canvasGroup, 1f, 0f, 1.0f, () => {
                taskUIPanel.SetActive(false);
            }));
        }
        else
        {
            taskUIPanel.SetActive(false);
        }
    }
    
    private IEnumerator FadeTaskUI(CanvasGroup canvasGroup, float fromAlpha, float toAlpha, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        
        canvasGroup.alpha = toAlpha;
        onComplete?.Invoke();
    }
    
    public void ShowPasswordNotebook()
    {
        // 1. 显示密码本封面
        if (diaryCoverUI != null)
        {
            diaryCoverUI.SetActive(true);
        }
        
        // 2. 显示密码本解锁对话
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text="5，2，8....",
                    speakerName="主控",
                    portrait=adult_surprised // MC_5
                },
                new DialogueLine{ 
                    text="密码本打开了！",
                    speakerName="主控",
                    portrait=adult_surprised // MC_5
                }
            }
        };
        
        ShowDialogue(dialogue, () => {
            // 3. 对话结束后，关闭封面，显示展开的日记本
            if (diaryCoverUI != null)
                diaryCoverUI.SetActive(false);
                
            if (diaryManager != null)
                diaryManager.ShowDiary();
            else if (diaryUI != null)
                diaryUI.SetActive(true);
                
            // 4. 标记日记已解锁
            isDiaryUnlocked = true;
        });
    }
    
    public void ClosePasswordNotebook()
    {
        // 这个方法现在不需要了，因为已删除passwordNotebookUI字段
        // 保留方法以防有地方调用
    }
    
    public void OpenDiary()
    {
        // 显示日记本UI
        if (diaryUI != null)
            diaryUI.SetActive(true);
    }
    
    public void CloseDiary()
    {
        if (diaryManager != null)
            diaryManager.CloseDiary();
        else if (diaryUI != null)
            diaryUI.SetActive(false);
    }
    
    public void ShowDialogue(DialogueLine line)
    {
        ShowDialogue(new DialogueSession{
            lines = new DialogueLine[] { line }
        });
    }
    
    public void ShowDialogue(DialogueSession session, System.Action onComplete = null)
    {
        if (dialoguePanel == null) return;
        
        dialoguePanel.SetActive(true);
        StartCoroutine(DisplayDialogueSequence(session, onComplete));
    }
    
    private IEnumerator DisplayDialogueSequence(DialogueSession session, System.Action onComplete = null)
    {
        foreach (var line in session.lines)
        {
            if (speakerNameText != null)
                speakerNameText.text = line.speakerName;
                
            if (speakerPortrait != null && line.portrait != null)
                speakerPortrait.sprite = line.portrait;
                
            if (dialogueText != null)
            {
                // 打字机效果
                yield return StartCoroutine(TypeText(dialogueText, line.text));
                
                // 等待玩家点击继续
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                yield return null; // 等待一帧避免连续触发
            }
        }
        
        dialoguePanel.SetActive(false);
        onComplete?.Invoke();
    }
    
    private IEnumerator TypeText(TextMeshProUGUI textComponent, string text)
    {
        textComponent.text = "";
        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    // UI按钮事件
    public void OnPasswordNotebookClicked()
    {
        ShowPasswordNotebook();
    }
    
    public void OnDiaryCloseClicked()
    {
        CloseDiary();
    }
}