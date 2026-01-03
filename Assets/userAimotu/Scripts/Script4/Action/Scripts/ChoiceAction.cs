using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ChoiceAction",menuName = "Game/State Actions/Choice Action")]
public class ChoiceAction : StateAction
{
    public string question;
    public string[] options;
    public List<ActionListContainer> branchActions; // 每个选项对应一组 Action

    public override IEnumerator Execute()
    {
        int selectedIndex = -1;
        ChoiceUISystem.Instance.Show(question, options, (index) => selectedIndex = index);
        while (selectedIndex == -1) yield return null;

        // 执行对应分支里的所有 Action
        if (selectedIndex >= 0 && selectedIndex < branchActions.Count)
        {
            foreach (var action in branchActions[selectedIndex].actions)
            {
                if (action != null) yield return action.Execute();
            }
        }
        else
        {
            Debug.LogWarning("未配置对应选项的 Action 分支");
        }
    }
}
