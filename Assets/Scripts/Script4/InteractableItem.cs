
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
    [Header("交互行为")]
    public bool allowRepeatClick = true;

    [Header("鸭子音效")]
    public AudioSource audioSource;
    public AudioClip quackSound;


    [Header("1. 基础设置")]
    public ItemType type; // 物品类型
    public GameObject questionMarkIcon; // 靠近时显示的问号图标
   
    [Header("2. 弹板配置 (可选)")]
    public bool hasPopup = false;   // 是否有线索大图
    public GameObject popupPanel;       // 拖入线索面板
    public string popupText = "默认线索描述内容"; // 可以在 Inspector 中设置或动态修改


    [Header("3. 对话配置 (可选)")]
    public bool hasDialogue = true; // 是否有对话
    public DialogueSession dialogueData; // 拖入或填写对话内容
    public Sprite popupSprite;          //对话立绘

    private bool canInteract = false; // 是否在交互范围内
    private bool isCollected = false; //防止重复触发
    void Start()
    {
        if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
    }

    // === 鼠标点击逻辑 ===
    private void OnMouseDown()
    {

        // 1. 基础检查：是否被UI遮挡？是否在范围内？是否正在对话中？
        if (GameManager.Instance.IsUIBlocking) return;

        if (!canInteract || DialogueManager.instance.IsDialogueActive) return;
        if (!allowRepeatClick && isCollected) return;
        // 如果当前正在对话中，不允许触发
        if (!canInteract || DialogueManager.instance.IsDialogueActive) return;




        if (type == ItemType.Dusk)
        {
            audioSource.PlayOneShot(quackSound);
            return;
        }
        if (type == ItemType.Note)
        {
            if (GameManager.Instance.taskManager.IsNoteViewed())
            {
                // 已查看过，不再触发
                return;
            }

            if (hasPopup && popupPanel)
            {
                popupPanel.SetActive(true);

                // 设置内容
                TextMeshProUGUI textComp = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null) textComp.text = popupText.Replace(@"\n", "\n");

                // 关闭按钮
                Button closeBtn = popupPanel.GetComponentInChildren<Button>();
                if (closeBtn != null)
                {
                    closeBtn.onClick.RemoveAllListeners();
                    closeBtn.onClick.AddListener(() =>
                    {
                        popupPanel.SetActive(false);
                        GameManager.Instance.taskManager.ViewNote(); // 标记已查看
                        GameManager.Instance.EnterState(GameManager.RoomState.PasswordCollecting);
                    });
                }
                return;
            }
        }
        if (type == ItemType.NoteBook)
        {
            if (hasPopup && popupPanel)
            {
                popupPanel.SetActive(true);
                TextMeshProUGUI textComp = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null) textComp.text = popupText.Replace(@"\n", "\n");

                Button closeBtn = popupPanel.GetComponentInChildren<Button>();
                if (closeBtn != null)
                {
                    closeBtn.onClick.RemoveAllListeners();
                    closeBtn.onClick.AddListener(() => popupPanel.SetActive(false));
                }
                return;
            }
        }


        // 通用流程：弹板 -> 对话 -> 结束通知
        if (hasPopup && popupPanel)
        {       
            popupPanel.SetActive(true);

            // 动态设置弹板文本
            TextMeshProUGUI textComponent = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = popupText.Replace(@"\n", "\n"); // 动态内容
            }

            // 关闭按钮逻辑
            Button closeButton = popupPanel.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    popupPanel.SetActive(false);
                    StartConversation();
                });
            }                          
        }
        else
        {
            // 没有弹板，直接对话
            StartConversation();
        }
        

    }

    // === 处理对话 ===
    private void StartConversation()
    {
     //   Debug.Log($"DialogueManager.instance = {DialogueManager.instance}");
       // Debug.Log($"dialogueData = {dialogueData}");

        if (hasDialogue)
        {
            // 开始对话，对话结束后调用 OnFinished
            DialogueManager.instance.StartDialogue(dialogueData, OnFinished);
        }
        else
        {
            OnFinished();
        }
    }

    // === 流程结束 ===
    private void OnFinished()
    {
        GameManager.Instance.OnItemInteracted(type);
        
        if (!allowRepeatClick)
        {
            isCollected = true;
            
        }
    }

    // === 范围检测 ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(true);
        }
        Debug.Log($"进入交互范围: {type}");

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
        }
    }
}
