using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlUIAction", menuName = "Game/State Actions/Control UI")]
public class ControlUIAction : StateAction
{
    public enum UIType { Notebook, NoteBigImage, Map }
    public UIType targetUI;
    public bool isClose = false; // 新增：是否是关闭操作
    [Header("音效设置")]
    public AudioClip customSFX;
    [Range(0f, 1f)]
    public float volume = 1.0f; // 新增：音量控制
    public override IEnumerator Execute()
    {
        var manager = GetManager();
        manager?.PlayGlobalSFX(customSFX, volume);
        switch (targetUI)
        {
            case UIType.Notebook:
                HandleNotebook(manager);
                break;
            case UIType.NoteBigImage:
                HandleNoteBigImage(manager);
                break;
            case UIType.Map:
                HandleMap(manager);
                break;
        }
      
        yield return null;
    }
    private void HandleNotebook(IGameManager manager)
    {
    
        if (isClose)
        {
            NotebookUI.Instance.Close();
            manager?.PopUIBlock("Notebook"); // 移除阻断
        }
        else
        {
            manager?.PushUIBlock("Notebook"); // 增加阻断
            NotebookUI.Instance.Open();
        }
    }
    private void HandleNoteBigImage(IGameManager manager)
    {
        if (isClose)
        {
            NoteUI.Instance.Close();
            manager?.PopUIBlock("Notebook"); // 移除阻断
        }
        else
        {
            manager?.PushUIBlock("Notebook"); // 增加阻断
            NoteUI.Instance.Open();
        }
    }
    private void HandleMap(IGameManager manager)
    {
        if (isClose)
        {
            // 假设你的地图 UI 类名叫 MapUI
            MapUI.Instance.Close();
            manager?.PopUIBlock("Map");
        }
        else
        {
            manager?.PushUIBlock("Map");
            MapUI.Instance.Open();
        }
    }
}
