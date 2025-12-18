using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro; // 引入 TextMesh Pro 的命名空间

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // 对话框面板
    [SerializeField] private Text dialogueText; // 对话文本
    [SerializeField] private Button nextButton; // 下一条按钮

    private List<string> dialogues = new List<string>(); // 对话内容列表
    private int currentDialogueIndex = 0;
    private bool isDialogueActive = false; // 用于标记对话是否正在进行

    private void Start()
    {
        // 初始化：隐藏对话框
        dialoguePanel.SetActive(false);

        // 绑定按钮点击事件
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(ShowNextDialogue);
        }
    }

    private void Update()
    {
        // 监听输入事件（按下 Space 或鼠标左键）
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            ShowNextDialogue();
        }
    }

    /// <summary>
    /// 加载对话文件（JSON 格式）
    /// </summary>
    /// <param name="filePath">对话文件路径</param>
    public void LoadDialogueFile(string filePath)
    {
        try
        {
            // 从文件加载 JSON 数据
            string jsonContent = File.ReadAllText(filePath);

            // 解析 JSON 数据
            DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);
            dialogues = new List<string>(dialogueData.dialogues);

            Debug.Log("对话文件加载成功！");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"加载对话文件失败：{ex.Message}");
        }
    }

    /// <summary>
    /// 开始对话
    /// </summary>
    public void StartDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.LogWarning("没有可用的对话内容！");
            return;
        }

        currentDialogueIndex = 0; // 重置对话索引
        isDialogueActive = true; // 标记对话开始
        dialoguePanel.SetActive(true); // 显示对话框
        ShowNextDialogue(); // 显示第一条对话
    }

    /// <summary>
    /// 显示下一条对话
    /// </summary>
    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            dialogueText.text = dialogues[currentDialogueIndex]; // 更新对话内容
            currentDialogueIndex++; // 移动到下一条对话
        }
        else
        {
            EndDialogue(); // 对话结束
        }
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // 隐藏对话框
        isDialogueActive = false; // 标记对话结束
        Debug.Log("对话结束！");
    }

    // 数据结构：用于解析 JSON 文件
    [System.Serializable]
    private class DialogueData
    {
        public string[] dialogues;
    }
}