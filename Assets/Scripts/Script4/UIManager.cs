/*
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("弹板 UI 组件")]
    public GameObject cluePanel;   // 弹板整体
    public Image clueImage;        // 弹板里的图片
    public Button closeButton;     // 关闭按钮

    [Header("任务UI")]
    public TextMeshProUGUI passwordTaskText; // 密码任务的文本框
    public TextMeshProUGUI diaryTaskText;    // 日记任务的文本框

    private Action onCloseCallback; // 关闭弹板后要做的事

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (cluePanel != null) cluePanel.SetActive(false);

        // 绑定关闭按钮事件
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCluePanel);
        }
    }
    private void Start()
    {
        // 确保面板关闭
        if (cluePanel != null) cluePanel.SetActive(false);

        // 监听关闭按钮点击事件
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                CloseCluePanel();
            });
        }
    }
    // 显示弹板
    public void ShowClue(Sprite clueSprite, Action callback = null)
    {
        if (cluePanel != null)
        {
            cluePanel.SetActive(true);
        }

        if (clueImage != null && clueSprite != null)
        {
            clueImage.sprite = clueSprite;
        }

        onCloseCallback = callback; // 记住关闭时要干嘛（比如开始对话）
    }

    // 关闭弹板
    public void CloseCluePanel()
    {
        if (cluePanel != null && cluePanel.activeSelf)
        {
            cluePanel.SetActive(false);

            // 执行回调（通常是开始对话）
            onCloseCallback?.Invoke();
            onCloseCallback = null;
        }
    }
    // 更新任务UI的方法
    public void UpdateTaskUI(string taskDescription, bool isPasswordTask = true)
    {
        if (isPasswordTask && passwordTaskText != null)
        {
            passwordTaskText.text = taskDescription; // 更新密码任务文本
        }
        else if (!isPasswordTask && diaryTaskText != null)
        {
            diaryTaskText.text = taskDescription; // 更新日记任务文本
        }
    }
}
*/