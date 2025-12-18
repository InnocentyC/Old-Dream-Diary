using UnityEngine;
using TMPro; 
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // 单例模式，方便全局调用

    [Header("UI 组件")]
    public TextMeshProUGUI taskText;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject itemPanel; // 用于显示放大物品

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        itemPanel.SetActive(false);
    }

    // 更新左上角任务
    public void UpdateTaskUI(int passwordCount, int diaryCount)
    {
        string task1 = $"找到 {passwordCount} / 3 位密码 " + (passwordCount >= 3 ? "√" : "");
        string task2 = $"找回 {diaryCount} / 1 篇日记 " + (diaryCount >= 1 ? "√" : "");
        taskText.text = task1 + "\n" + task2;
    }

    // 显示对话（打字机效果）
    public void ShowDialogue(string content, float duration = 3f)
    {
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(content, duration));
    }

    IEnumerator TypeDialogue(string content, float duration)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        foreach (char letter in content.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);//打字效果速度
        }
        yield return new WaitForSeconds(10f);//对话展示时间——这里之后改成点击才换
        dialoguePanel.SetActive(false); // 对话结束后关闭
    }

    // 显示物品大图（简化版，只开关面板）
    public void ShowItemDetail(bool show)
    {
        itemPanel.SetActive(show);
    }
}
