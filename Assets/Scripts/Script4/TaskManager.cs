using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI passwordTaskText;
    public TextMeshProUGUI diaryTaskText;

    // 每个位的密码状态
    private bool fishCollected = false;  // 热带鱼
    private bool dollCollected = false;  // 玩偶
    private bool awardCollected = false; // 奖状
    private bool diaryCollected = false; // 鱼形串珠日记
    private bool noteViewed = false; // 便利贴是否已查看过

    private const int PASSWORD_TOTAL = 3;
    private const int DIARY_TOTAL = 1;

    private void Start()
    {
       
    }

    // 修改：每个密码位独立记录，防止玩家重复点击同一个物品导致计数错误。
    public void CollectPassword(ItemType type)
    {
        switch (type)
        {
            case ItemType.FishTank:
                if (!fishCollected) fishCollected = true;
                break;
            case ItemType.Doll:
                if (!dollCollected) dollCollected = true;
                break;
            case ItemType.Award:
                if (!awardCollected) awardCollected = true;
                break;
        }
        UpdateTaskUI();
    }

    public void CollectDiary()
    {
        if (!diaryCollected)
        {
            diaryCollected = true;
            UpdateTaskUI();
        }
    }

    public void ViewNote()
    {
        if (noteViewed) return;
        noteViewed = true;
        // 第一次查看 Note 时可以触发提示逻辑
    }
    public bool IsNoteViewed() => noteViewed;
    public void ShowTaskUI()
    {
        passwordTaskText.gameObject.SetActive(true);
        diaryTaskText.gameObject.SetActive(true);
        UpdateTaskUI();
    }

    private void UpdateTaskUI()
    {
        int passwordCount = (fishCollected ? 1 : 0) + (dollCollected ? 1 : 0) + (awardCollected ? 1 : 0);
        passwordTaskText.text = $"找到 {passwordCount} / {PASSWORD_TOTAL}位密码";
        if (passwordCount == PASSWORD_TOTAL) passwordTaskText.text += " √";

        int diaryCount = diaryCollected ? 1 : 0;
        diaryTaskText.text = $"找回 {diaryCount} / {DIARY_TOTAL}篇日记";
        if (diaryCount == DIARY_TOTAL) diaryTaskText.text += " √";
    }
    public bool AreAllTasksCompleted()
    {
        return fishCollected && dollCollected && awardCollected && diaryCollected;
    }

}
