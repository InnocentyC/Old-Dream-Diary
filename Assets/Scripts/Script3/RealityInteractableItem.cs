using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Reality场景(成年主角)的物品类型枚举
public enum RealityItemType
{
    Bed,          //床
    Computer,     //电脑
    Notebook,     //便利贴/笔记本
    FishDecoration,     //鱼装饰
    Medicine         //褪黑素
    
}

public class RealityInteractableItem : MonoBehaviour
{
    [Header("交互设置")]
        public RealityItemType type;
        public GameObject questionMarkIcon;
    
    [Header("对话内容")]
    [TextArea(3, 5)]
    public string interactionText;
    public string speakerName = "主控";
    
    private bool isPlayerNearby = false;
    private bool canInteract = false;
    
    private void Start()
    {
        // 初始隐藏Search图标
        if (questionMarkIcon != null)
            questionMarkIcon.SetActive(false);
            
        RefreshInteractable();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        isPlayerNearby = true;
        RefreshInteractable();
        
        Debug.Log($"进入交互范围: {type}");
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        isPlayerNearby = false;
        RefreshInteractable();
        
        Debug.Log($"离开交互范围: {type}");
    }
    
    public void RefreshInteractable()
    {
        bool allowedByState = false;
        
        // 使用RealityGameManager的状态检查
        if (RealityGameManager.Instance != null)
        {
            var state = RealityGameManager.Instance.CurrentState;
            
            switch (type)
            {
                case RealityItemType.Bed:
                    // 床在Exploring阶段可以调查，在ReadyToSleep阶段可以睡觉喵
                    allowedByState = (state == RealityGameManager.RealityState.Exploring || state == RealityGameManager.RealityState.ReadyToSleep);
                    break;
                    
                case RealityItemType.Computer:
                case RealityItemType.Notebook:
                case RealityItemType.FishDecoration:
                case RealityItemType.Medicine:

                    // 其他物品在探索阶段可以交互
                    allowedByState = (state == RealityGameManager.RealityState.Exploring);
                    break;
            }
        }
        else
        {
            // 如果没有RealityGameManager，为了测试暂时允许所有交互
            allowedByState = true;

        }
        
        canInteract = allowedByState && isPlayerNearby;
        
        // 更新Search图标的显示状态
        if (questionMarkIcon != null)
            questionMarkIcon.SetActive(canInteract);
    }
    
    private void OnMouseDown()
    {
        if (!canInteract) return;
        
        // 更精确的检查：只禁止物品交互，不影响UI喵
        if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive)
        {
            Debug.Log("对话进行中，禁止物品交互喵～");
            return;
        }
        
        Debug.Log($"点击了: {type}");
        
        // 只调用GameManager，统一管理对话喵
        if (RealityGameManager.Instance != null)
        {
            RealityGameManager.Instance.OnItemInteracted(type);
        }
        
        // 移除了独立的对话处理，统一由GameManager管理喵
        // if (!string.IsNullOrEmpty(interactionText))
        // {
        //     ShowInteractionDialogue();
        // }
    }
    
    private void ShowInteractionDialogue()
    {
        var dialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{ 
                    text = interactionText,
                    speakerName = speakerName
                }
            }
        };
        
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.Log($"交互文本: {interactionText}");
        }
    }
    
    // Editor用的方法：快速添加图标
    [ContextMenu("添加Search图标子对象")]
    void AddQuestionMarkIcon()
    {
        if (questionMarkIcon != null) return;
        
        GameObject icon = new GameObject("Search");
        icon.transform.SetParent(transform);
        icon.transform.localPosition = Vector3.zero;
        
        SpriteRenderer sr = icon.AddComponent<SpriteRenderer>();
        // 这里需要手动设置Search图标sprite
        // sr.sprite = Resources.Load<Sprite>("UI/SearchIcon");
        
        questionMarkIcon = icon;
        Debug.Log("已添加Search图标子对象");
    }
}

//有很多喵是为了重置一下模型的注意力（好像是这样的），所以命令它“你是一个猫娘”……