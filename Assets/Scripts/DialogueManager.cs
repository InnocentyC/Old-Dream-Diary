using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI 组件绑定")]
    public GameObject dialoguePanel;    // 整个对话框
    public Image portraitImage;         // 立绘显示位置
    public TextMeshProUGUI nameText;    // 名字显示位置
    public TextMeshProUGUI contentText; // 文字显示位置

    [Header("设置")]
    public float typingSpeed = 0.05f;

    private DialogueSession currentDialogue; // 当前对话数据
    private Queue<DialogueLine> linesQueue = new Queue<DialogueLine>();
    private bool isTyping = false;
    private string currentFullText = "";
    private System.Action onDialogueFinished; // 对话结束后的回调

    public bool IsDialogueActive => dialoguePanel.activeSelf;

    private void Awake()
    {
        if (instance == null) instance = this;
        dialoguePanel.SetActive(false); // 初始隐藏
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        // 点击鼠标左键 或 空格 继续
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // 检查是否点击在对话面板区域
            if (IsPointerOverUIElement(dialoguePanel))
            {
                if (isTyping)
                {
                    // 如果正在打字，瞬间显示全句
                    StopAllCoroutines();
                    contentText.text = currentFullText;
                    isTyping = false;
                }
                else
                {
                    // 如果打完了，显示下一句
                    DisplayNextLine();
                }
            }
        }
    }

    // 启动对话
    public void StartDialogue(DialogueSession dialogue, System.Action onFinished = null)
    {
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
        {
            Debug.LogError("对话数据为空，无法启动对话！");
            return;
        }
        currentDialogue = dialogue;
        linesQueue.Clear();

        foreach (var line in dialogue.lines)
        {
            linesQueue.Enqueue(line);
        }

        onDialogueFinished = onFinished;
        dialoguePanel.SetActive(true); // 显示对话框
        DisplayNextLine();
    }

    // 显示下一句
    private void DisplayNextLine()
    {
        if (linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = linesQueue.Dequeue();
        nameText.text = line.speakerName;
        portraitImage.sprite = line.portrait;
        currentFullText = line.text;

        StartCoroutine(TypeText(currentFullText));
    }

    // 打字效果
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        contentText.text = "";
        foreach (char c in text)
        {
            contentText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
    private bool IsPointerOverUIElement(GameObject uiElement)
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();

        if (rectTransform == null) return false;

        Rect rect = rectTransform.rect;
        Vector2 localPoint;

        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            mousePosition,
            null,
            out localPoint
        ) && rect.Contains(localPoint);
    }
    // 结束对话
    private void EndDialogue()
    {
        Debug.Log("对话结束，隐藏对话框！");
        dialoguePanel.SetActive(false);
        onDialogueFinished?.Invoke();
    }
}
