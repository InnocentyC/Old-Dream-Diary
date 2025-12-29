
using UnityEngine;


//InteractableItem 只做三件事：
//能不能点（距离 / 状态）
//点了是什么类型
//把“请求”交给系统
public enum ItemType
{
    Bed,          //床
    NoteBook,     //密码本
    Note,         //便利贴
    FishTank,     //鱼缸
    Doll,         //熊玩偶
    Award,        //奖状
    Beads,        //串珠
    Duck          //橡皮鸭  
}


public class InteractableItem : MonoBehaviour
{
    
    [Header("点击设置")]
    public bool allowRepeatClick = true;

    [Header("鸭子音效")]
    public AudioSource audioSource;
    public AudioClip quackSound;


 
   
    [Header("2. 弹板配置 (可选)")] //只能用于世界物体，不可用于UI内按钮
    public bool hasPopup = false;   // 是否有弹板
    //public GameObject popupPanel;       // 拖入线索面板
    [TextArea(3, 6)]
    public string popupText = "默认线索描述内容"; // 可以在 Inspector 中设置或动态修改


    [Header("3. 对话配置 (可选)")]
    public bool hasDialogue = true; // 是否有对话
    public DialogueSession dialogueData; // 拖入或填写对话内容
    public Sprite popupSprite;          //对话立绘
    public StickerType stickerType = StickerType.None;


    
    
    [Header("1. 基础设置")]
    public ItemType type; // 物品类型
    public GameObject questionMarkIcon; // 靠近时显示的问号图标
    public RoomStateEvent[] stateEvents; // 用于数据化事件
    private bool canInteract = false; // 是否在交互范围内
    private bool isCollected = false; //防止重复触发
    private bool isPlayerNearby = false;

    void Start()
    {
        if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
        
    }
   
    // === 鼠标点击逻辑 ===
    private void OnMouseDown()
    {
        /*
        if (GameManager.Instance != null)
        {
            return;
        }
        */
        if (GameManager.Instance.IsUIBlocking)
        {
            Debug.Log("[BLOCKED] by UIBlock");
            return;
        }
        Debug.Log($"[CLICK] {type} OnMouseDown| UIBlock={GameManager.Instance.uiBlockCount}");
        
        if (!canInteract)
        {
            Debug.Log("[BLOCKED] not in range");
            return;
        }
        
        if (DialogueManager.instance.IsDialogueActive)
        {
            Debug.Log("[BLOCKED] dialogue");
            return;
        }
        
        if (!allowRepeatClick && isCollected) return;

        Debug.Log("[PASS] base checks");

        if (type == ItemType.Duck)
        {
            if (audioSource != null && quackSound != null)
                audioSource.PlayOneShot(quackSound);
            return;
        }
        if (type == ItemType.NoteBook)
        {
            if (!NotebookUI.Instance.HasOpenedOnce)
            {
                // 第一次打开 Notebook → 播放 hint
                NotebookUI.Instance.Open();
            }
            else
            {
                // 第二次及以后 → 可以点击便利贴
                NotebookUI.Instance.OnStickyNoteClicked();
            }
            return;
        }

        if (hasPopup)
        {
            Debug.Log("[POPUP] open popup");
            PopupSystem.Instance.Open(
                popupText,
                stickerType,
                StartConversation   // 关闭 Popup 后开始对话
            );
            return;

        }
        GameManager.Instance.RequestInteraction(type, this);


       // StartConversation();  
        
    }

    public void TriggerClick()
    {
        OnMouseDown(); // 调用原本的点击逻辑
    }





    #region === Dialogue ===

    private void StartConversation()
    {
        if (hasDialogue && dialogueData != null)
        {
            DialogueManager.instance.StartDialogue(dialogueData, OnFinished);
        }
        else
        {
            OnFinished();
        }
    }

    private void OnFinished()
    {
        GameManager.Instance.OnItemInteracted(type,this);

        if (!allowRepeatClick)
            isCollected = true;
      
    }

    #endregion
  


    #region === Range & State ===
    public void RefreshInteractable()
    {
        bool allowedByState = false;
        var state = GameManager.Instance.CurrentState;

        switch (type)
        {
           
            case ItemType.NoteBook:
            case ItemType.Beads:
                allowedByState = (state != GameManager.RoomState.Intro &&
                                  state != GameManager.RoomState.ReadyToExit);
                break;

            case ItemType.FishTank:
            case ItemType.Doll:
            case ItemType.Award:
                allowedByState = (state == GameManager.RoomState.PasswordCollecting ||
                                  state == GameManager.RoomState.AllTasksDone);
                break;

            case ItemType.Bed:
                allowedByState = (state != GameManager.RoomState.Intro &&
                                  state != GameManager.RoomState.ReadyToExit);
                break;
        }

        canInteract = isPlayerNearby && allowedByState;

        if (questionMarkIcon != null)
            questionMarkIcon.SetActive(canInteract);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
       // Trigger 只记录“玩家在范围内”
        isPlayerNearby = true;
        RefreshInteractable();
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isPlayerNearby = false;
        RefreshInteractable();
        
    }   
    #endregion
}
