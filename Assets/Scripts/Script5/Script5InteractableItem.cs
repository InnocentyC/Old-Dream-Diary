using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Script5场景的物品类型枚举
public enum Script5ItemType
{
    PasswordNotebook,    // 密码本
    Computer,           // 电脑
    Window,            // 落地窗
    Medicine,          // 褪黑素
    FishDecoration,    // 热带鱼摆件
    Phone,             // 手机
    Bed                // 床
}

public class Script5InteractableItem : MonoBehaviour
{
    [Header("交互设置")]
    public Script5ItemType type;
    public GameObject questionMarkIcon;
    
    [Header("交互顺序控制")]
    public bool requiresDiaryUnlocked = false; // 是否需要先解锁日记
    
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
        
        // 使用Script5GameManager的状态检查
        if (Script5GameManager.Instance != null)
        {
            // 检查是否需要日记解锁
            if (requiresDiaryUnlocked && !IsDiaryUnlocked())
            {
                allowedByState = false;
            }
            else
            {
                // 根据物品类型判断是否可交互
                switch (type)
                {
                    case Script5ItemType.PasswordNotebook:
                        // 密码本始终可交互
                        allowedByState = true;
                        break;
                        
                    case Script5ItemType.Computer:
                    case Script5ItemType.Window:
                    case Script5ItemType.Medicine:
                    case Script5ItemType.FishDecoration:
                        // 这些物品需要日记解锁后才能交互
                        allowedByState = IsDiaryUnlocked();
                        break;
                        
                    case Script5ItemType.Phone:
                        // 手机需要所有其他交互完成后才能交互
                        allowedByState = IsDiaryUnlocked() && AreAllOtherInteractionsComplete();
                        break;
                        
                    case Script5ItemType.Bed:
                        // 床需要所有交互完成后才能交互
                        allowedByState = AreAllInteractionsComplete();
                        break;
                }
            }
        }
        else
        {
            // 如果没有Script5GameManager，为了测试暂时允许所有交互
            allowedByState = true;
            Debug.LogWarning("未找到Script5GameManager，允许所有交互进行测试");
        }
        
        canInteract = allowedByState && isPlayerNearby;
        
        // 更新Search图标的显示状态
        if (questionMarkIcon != null)
            questionMarkIcon.SetActive(canInteract);
    }
    
    private bool IsDiaryUnlocked()
    {
        if (Script5GameManager.Instance != null)
        {
            return Script5GameManager.Instance.IsDiaryUnlocked();
        }
        return false;
    }
    
    private bool AreAllOtherInteractionsComplete()
    {
        // 检查除手机和床之外的所有交互是否完成
        var requiredItems = new Script5ItemType[] {
            Script5ItemType.PasswordNotebook,
            Script5ItemType.Computer,
            Script5ItemType.Window,
            Script5ItemType.Medicine,
            Script5ItemType.FishDecoration
        };
        
        foreach (var itemType in requiredItems)
        {
            var item = FindObjectsOfType<Script5InteractableItem>();
            bool found = false;
            foreach (var interactable in item)
            {
                if (interactable.type == itemType && interactable.IsInteractionCompleted())
                {
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }
        
        return true;
    }
    
    private bool AreAllInteractionsComplete()
    {
        // 检查所有交互是否完成
        var allItems = FindObjectsOfType<Script5InteractableItem>();
        foreach (var item in allItems)
        {
            if (!item.IsInteractionCompleted())
                return false;
        }
        return true;
    }
    
    private void OnMouseDown()
    {
        if (!canInteract) return;
        
        // 更精确的检查：只禁止物品交互，不影响UI
        if (Script5GameManager.Instance != null && Script5GameManager.Instance.IsUIBlocked())
        {
            Debug.Log("UI显示中，禁止物品交互");
            return;
        }
        
        Debug.Log($"点击了: {type}");
        
        // 调用Script5GameManager处理交互
        if (Script5GameManager.Instance != null)
        {
            Script5GameManager.Instance.OnItemInteracted(type);
        }
    }
    
    private bool IsInteractionCompleted()
    {
        if (Script5GameManager.Instance != null)
        {
            return Script5GameManager.Instance.IsInteractionCompleted(type);
        }
        return false;
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