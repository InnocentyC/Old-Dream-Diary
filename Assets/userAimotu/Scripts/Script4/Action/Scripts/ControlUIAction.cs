using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlUIAction", menuName = "Game/State Actions/Control UI")]
public class ControlUIAction : StateAction
{
    public enum UIType { Notebook, NoteBigImage }
    public UIType targetUI;
    public bool isClose = false; // 新增：是否是关闭操作

    public override IEnumerator Execute()
    {
        if (targetUI == UIType.Notebook)
        {
            if (isClose)
            {
                NotebookUI.Instance.Close();
                GameManager.Instance.PopUIBlock("Notebook"); // 移除阻断
            }
            else
            {
                GameManager.Instance.PushUIBlock("Notebook"); // 增加阻断
                NotebookUI.Instance.Open();
            }
        }
        else if (targetUI == UIType.NoteBigImage)
        {
            if (isClose)
            {
                NoteUI.Instance.Close();
                GameManager.Instance.PopUIBlock("Notebook"); // 移除阻断
            }
            else
            {
                GameManager.Instance.PushUIBlock("Notebook"); // 增加阻断
                NoteUI.Instance.Open();
            }
        }
        yield return null;
    }
}
