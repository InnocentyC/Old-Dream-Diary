using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI passwordTaskText;
    public TextMeshProUGUI diaryTaskText;

    private int passwordPiecesFound = 0;
    private int diaryFound = 0;

    private const int PASSWORD_TOTAL = 3;
    private const int DIARY_TOTAL = 1;

    private void Start()
    {
        UpdateTaskUI();
    }

    public void FindPasswordPiece()
    {
        passwordPiecesFound++;
        UpdateTaskUI();
    }

    public void FindDiary()
    {
        diaryFound++;
        UpdateTaskUI();
    }

    private void UpdateTaskUI()
    {
        passwordTaskText.text = $"找到 {passwordPiecesFound} / {PASSWORD_TOTAL}位密码";
        diaryTaskText.text = $"找回 {diaryFound} / {DIARY_TOTAL}篇日记";

        if (passwordPiecesFound == PASSWORD_TOTAL)
        {
            passwordTaskText.text += " √";
        }

        if (diaryFound == DIARY_TOTAL)
        {
            diaryTaskText.text += " √";
        }
    }
    public bool AreAllTasksCompleted()
    {
        return passwordPiecesFound == PASSWORD_TOTAL && diaryFound == DIARY_TOTAL;
    }
}
