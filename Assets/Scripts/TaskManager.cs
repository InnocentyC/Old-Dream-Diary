using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public Text taskText;
    private int passwordsFound = 0;
    private int totalPasswords = 3;
    private int diariesFound = 0;
    private int totalDiaries = 1;

    void Start()
    {
        UpdateTaskUI();
    }

    public void AddPasswordFound()
    {
        passwordsFound++;
        UpdateTaskUI();
    }

    public void AddDiaryFound()
    {
        diariesFound++;
        UpdateTaskUI();
    }

    private void UpdateTaskUI()
    {
        taskText.text = $"找到 {passwordsFound} / {totalPasswords} 位密码\n找回 {diariesFound} / {totalDiaries} 篇日记";
    }
}
