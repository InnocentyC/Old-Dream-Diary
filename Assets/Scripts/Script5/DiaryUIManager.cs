using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DiaryUIManager : MonoBehaviour
{
    public static DiaryUIManager Instance;
    
    [Header("日记本UI组件")]
    public GameObject diaryUI;
    public Button closeButton;
    
    [Header("左侧便利贴")]
    public Button stickyNoteButton;
    public GameObject stickyNoteInfoPanel; // 便利贴信息面板
    public TextMeshProUGUI stickyNoteTitleText;
    public TextMeshProUGUI stickyNoteDescText;
    public Button closeStickyNoteInfoButton;
    
    [Header("右侧日记内容")]
    public TextMeshProUGUI diaryTitleText;
    public Image diaryIconImage;
    public TextMeshProUGUI diaryContentText;
    
    [Header("翻页系统")]
    public Button prevPageButton;
    public Button nextPageButton;
    public TextMeshProUGUI pageNumberText;
    
    [Header("图标系统")]
    public Sprite fishBeadIcon; // 鱼形串珠图标
    public Sprite[] diaryIcons; // 其他日记图标（未解锁状态）
    
    [Header("便利贴信息")]
    public StickyNoteInfo[] stickyNoteInfos;
    
    [Header("日记内容")]
    public DiaryPage[] diaryPages;
    
    private int currentPage = 0;
    private bool isUIActive = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeDiaryUI();
    }
    
    private void InitializeDiaryUI()
    {
        // 初始化UI状态
        if (diaryUI != null)
            diaryUI.SetActive(false);
            
        if (stickyNoteInfoPanel != null)
            stickyNoteInfoPanel.SetActive(false);
        
        // 设置按钮事件
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDiary);
            
        if (stickyNoteButton != null)
            stickyNoteButton.onClick.AddListener(ShowStickyNoteInfo);
            
        if (closeStickyNoteInfoButton != null)
            closeStickyNoteInfoButton.onClick.AddListener(HideStickyNoteInfo);
            
        if (prevPageButton != null)
            prevPageButton.onClick.AddListener(PreviousPage);
            
        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);
            
        // 显示第一页
        currentPage = 0;
        UpdateDiaryPage();
    }
    
    public void ShowDiary()
    {
        if (diaryUI != null)
        {
            diaryUI.SetActive(true);
            isUIActive = true;
            
            // 重置到第一页
            currentPage = 0;
            UpdateDiaryPage();
        }
    }
    
    public void CloseDiary()
    {
        if (diaryUI != null)
        {
            diaryUI.SetActive(false);
            isUIActive = false;
            
            // 关闭便利贴信息面板
            HideStickyNoteInfo();
        }
    }
    
    public bool IsDiaryActive()
    {
        return isUIActive;
    }
    
    private void ShowStickyNoteInfo()
    {
        if (stickyNoteInfoPanel != null && stickyNoteInfos.Length > 0)
        {
            // 显示第一个便利贴的信息（热带鱼和串珠）
            var info = stickyNoteInfos[0];
            
            if (stickyNoteTitleText != null)
                stickyNoteTitleText.text = info.title;
                
            if (stickyNoteDescText != null)
                stickyNoteDescText.text = info.description;
                
            stickyNoteInfoPanel.SetActive(true);
        }
    }
    
    private void HideStickyNoteInfo()
    {
        if (stickyNoteInfoPanel != null)
            stickyNoteInfoPanel.SetActive(false);
    }
    
    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateDiaryPage();
        }
    }
    
    private void NextPage()
    {
        if (currentPage < diaryPages.Length - 1)
        {
            currentPage++;
            UpdateDiaryPage();
        }
    }
    
    private void UpdateDiaryPage()
    {
        if (currentPage >= diaryPages.Length)
            return;
            
        var page = diaryPages[currentPage];
        
        // 更新日记标题
        if (diaryTitleText != null)
            diaryTitleText.text = page.title;
            
        // 更新日记图标
        if (diaryIconImage != null)
        {
            if (currentPage == 0 && page.icon != null)
            {
                // 第一页显示鱼形串珠图标
                diaryIconImage.sprite = page.icon;
            }
            else if (diaryIcons.Length > currentPage - 1)
            {
                // 其他页显示未解锁图标
                diaryIconImage.sprite = diaryIcons[currentPage - 1];
            }
        }
        
        // 更新日记内容
        if (diaryContentText != null)
        {
            if (currentPage == 0)
            {
                // 只有第一页有日记内容
                diaryContentText.text = page.content;
                diaryContentText.gameObject.SetActive(true);
            }
            else
            {
                // 其他页面显示提示文字
                diaryContentText.text = "尚未解锁...";
                diaryContentText.gameObject.SetActive(true);
            }
        }
        
        // 更新页码
        UpdatePageNumber();
        
        // 更新按钮状态
        UpdateNavigationButtons();
    }
    
    private void UpdatePageNumber()
    {
        if (pageNumberText != null)
        {
            pageNumberText.text = $"{currentPage + 1}/{diaryPages.Length}";
        }
    }
    
    private void UpdateNavigationButtons()
    {
        // 更新上一页按钮
        if (prevPageButton != null)
        {
            prevPageButton.interactable = currentPage > 0;
        }
        
        // 更新下一页按钮
        if (nextPageButton != null)
        {
            nextPageButton.interactable = currentPage < diaryPages.Length - 1;
        }
    }
    
    // 便利贴信息点击事件
    public void OnStickyNoteIconClicked(int iconIndex)
    {
        if (iconIndex >= 0 && iconIndex < stickyNoteInfos.Length)
        {
            var info = stickyNoteInfos[iconIndex];
            
            if (stickyNoteTitleText != null)
                stickyNoteTitleText.text = info.title;
                
            if (stickyNoteDescText != null)
                stickyNoteDescText.text = info.description;
                
            stickyNoteInfoPanel.SetActive(true);
        }
    }
}

[System.Serializable]
public class StickyNoteInfo
{
    public string title;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon; // 便利贴上的图案
}

[System.Serializable]
public class DiaryPage
{
    public string title;
    [TextArea(5, 10)]
    public string content;
    public Sprite icon; // 日记标题旁的图标
}