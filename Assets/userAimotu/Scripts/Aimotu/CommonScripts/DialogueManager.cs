using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;



public class DialogueManager : MonoBehaviour
{
    private IGameManager GetManager()
    {
        // 增加容错获取
        var managers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mono in managers)
        {
            if (mono is IGameManager gm) return gm;
        }
        Debug.LogWarning("IGameManager 未找到！请检查场景中的 GameManager 是否勾选了脚本并实现了接口。");
        return null;
    }
    public static DialogueManager instance;
    public string Name;

    [Header("UI 组件")]
    public GameObject dialoguePanel;    // 整个对话框
    public Image portraitImage;         // 立绘显示位置
    public TextMeshProUGUI nameText;    // 名字显示位置
    public TextMeshProUGUI contentText; // 文字显示位置

    [Header("设置")]
    public float typingSpeed = 0.05f;
    

    private DialogueSession currentDialogue; // 当前对话数据
    [Header("默认主角立绘")]
    private Queue<DialogueLine> linesQueue = new Queue<DialogueLine>();
    private bool isTyping = false;
    private bool isDialogueActive = false;
    private string currentFullText = "";
    private System.Action onDialogueFinished; // 对话结束后的回调

    //判断对话是否能生效
    public bool IsDialogueActive => isDialogueActive;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false); // 初始隐藏
        isDialogueActive = false;
    }    
    // 启动对话
    public void StartDialogue(DialogueSession dialogue, System.Action onFinished = null)
    {
        if (isDialogueActive)
        {
            Debug.LogError("重复 StartDialogue，被拒绝");
            return;
        }

        if (dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
        {
            Debug.LogError("对话数据为空，无法启动对话！");
            return;
        }


        //进入对话
        isDialogueActive = true;
        var manager = GetManager();
        if (manager != null)
        {
            manager.PushUIBlock("Dialogue");
        }
        else
        {
            Debug.LogError("由于找不到 GameManager，UI 阻断可能失效，但对话继续尝试运行。");
        }

        onDialogueFinished = onFinished;
        linesQueue.Clear();
       

        foreach (var line in dialogue.lines)
        {
            linesQueue.Enqueue(line);
        }
       
        dialoguePanel.SetActive(true); // 显示对话框        
        DisplayNextLine();
    }
    public void PlayOneLine(string text, SpeakerNameOption speaker= SpeakerNameOption.None, PortraitOption portraitOption = PortraitOption.None)
    {
        // 如果当前正在完整对话中，不打断
        if (IsDialogueActive)
        {
            Debug.LogWarning("PlayOneLine 被忽略：已有对话在进行");
            return;
        }
        var currentLine = linesQueue.Dequeue();
        var tempDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
            new DialogueLine
            {
                speaker = speaker,
                text = text,
                portrait = portraitOption
            }
            }
        };
        Sprite portraitSprite = currentLine.GetPortrait(GetManager());
        Debug.Log($"[Dialogue] 尝试获取立绘: {currentLine.portrait}, 结果是否为空: {portraitSprite == null}");
    
        if (portraitSprite != null)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = portraitSprite;
        }

        StartDialogue(tempDialogue);
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

    // 显示下一句
    private void DisplayNextLine()
    {
        if (linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        var lines= linesQueue.Dequeue();
        // ===== 名字处理 =====
        string speakerName = lines.GetSpeakerName();
        if (string.IsNullOrEmpty(speakerName))
        {
            nameText.gameObject.SetActive(false);
        }
        else
        {
            nameText.gameObject.SetActive(true);
            nameText.text = speakerName;
        }
        Sprite portraitSprite = lines.GetPortrait(GetManager());

        if (portraitSprite != null)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = portraitSprite;
        }
        else
        {
            // 旁白 → 不显示立绘
            portraitImage.gameObject.SetActive(false);
        }

        currentFullText = lines.text;
        StopAllCoroutines();
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
        if (!isDialogueActive) return;

        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        GetManager().PopUIBlock("Dialogue");
        

        onDialogueFinished?.Invoke();
        onDialogueFinished = null;
    }
}
