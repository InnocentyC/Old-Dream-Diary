using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引入 TextMesh Pro 的命名空间

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel; // 对话框 Panel
    [SerializeField] private TMP_Text dialogueText;        // 对话内容
    [SerializeField] private Button nextButton;        // 下一句按钮
    [SerializeField] private Text speakerNameText;     // （可选）显示说话者名字的文本

    private void Awake()
    {
        // 单例模式
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
        // 初始化：隐藏对话框
        dialoguePanel.SetActive(false);

        // 设置按钮点击事件
        nextButton.onClick.AddListener(() =>
        {
            DialogueManager.Instance.ShowNextLine();
        });
    }

    // 显示对话框
    public void ShowDialogue(string line, string speakerName = "")
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = line;

        if (speakerNameText != null && !string.IsNullOrEmpty(speakerName))
        {
            speakerNameText.text = speakerName;
        }
    }

    // 隐藏对话框
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
