using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaSwitchAction", menuName = "Game/State Actions/Area Switch")]
public class AreaSwitchAction : StateAction
{
    [Header("目标场景名称")]
    public string targetSceneName;

    [Header("转场后进入的状态")]
    public RoomState nextState;

    public override IEnumerator Execute()
    {
        if (FadeManager.Instance == null)
        {
            Debug.LogError("场景中缺少 FadeManager!");
            yield break;
        }

        // 1. 调用 FadeManager 执行淡出并加载场景
        // 注意：这里需要修改 FadeManager 让它支持在加载后执行回调
        FadeManager.Instance.TransitionToScene(targetSceneName);

        // 2. 这里的逻辑会在场景开始加载时中断，属于正常现象
        // 场景加载后的初始状态设置建议放在新场景的管理器中
        yield return null;
    }

}