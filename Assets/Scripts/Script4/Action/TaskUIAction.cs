using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskUIAction", menuName = "Game/State Actions/TaskUI")]

public class TaskUIAction : StateAction
{
    public override IEnumerator Execute()
    {
        while (GameManager.Instance.IsUIBlocking)
            yield return null;
        GameManager.Instance.taskManager.ShowTaskUI();
        yield break;
    }
}

