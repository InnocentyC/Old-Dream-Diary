
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Bed,          //床
    NoteBook,     //密码本
    Note,         //便利贴
    FishTank,     //鱼缸
    Doll,         //熊玩偶
    Award,        //奖状
    Beads,        //串珠
    Dusk          //橡皮鸭  
}


public class InteractableItem : MonoBehaviour
{
    [Header("点击设置")]
    public bool allowRepeatClick = true;

    [Header("鸭子音效")]
    public AudioSource audioSource;
    public AudioClip quackSound;


    [Header("1. 基础设置")]
    public ItemType type; // 物品类型
    public GameObject questionMarkIcon; // 靠近时显示的问号图标
   
    [Header("2. 弹板配置 (可选)")] //只能用于世界物体，不可用于UI内按钮
    public bool hasPopup = false;   // 是否有弹板
    public GameObject popupPanel;       // 拖入线索面板
    public string popupText = "默认线索描述内容"; // 可以在 Inspector 中设置或动态修改


    [Header("3. 对话配置 (可选)")]
    public bool hasDialogue = true; // 是否有对话
    public DialogueSession dialogueData; // 拖入或填写对话内容
    public Sprite popupSprite;          //对话立绘

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

        // 1. 基础检查：是否被UI遮挡？是否在范围内？是否正在对话中？
 
        if (GameManager.Instance.IsUIBlocking || !canInteract || DialogueManager.instance.IsDialogueActive) return;
        if (!allowRepeatClick && isCollected) return;





        if (type == ItemType.Dusk)
        {
            if (audioSource != null && quackSound != null)
                audioSource.PlayOneShot(quackSound);
            return;
        }
        if (hasPopup && popupPanel != null)
        {
            OpenPopup();
        }
        else
        {
            StartConversation();
        }
    }
    #region === Popup ===

    private void OpenPopup()
    {
        popupPanel.SetActive(true);

        var text = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = popupText.Replace(@"\n", "\n");

        var closeBtn = popupPanel.GetComponentInChildren<Button>();
        if (closeBtn != null)
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(() =>
            {
                popupPanel.SetActive(false);
                StartConversation();
            });
        }
    }

    #endregion
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
        GameManager.Instance.OnItemInteracted(type);

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
                allowedByState = (state != GameManager.RoomState.Intro);
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
        
        Debug.Log($"进入交互范围: {type}");

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isPlayerNearby = false;
        RefreshInteractable();
        
    }   
    #endregion
}
