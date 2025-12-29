using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskUIAction", menuName = "Game/State Actions/TaskUI")]

public class TaskUIAction : StateAction
{
    public override IEnumerator Execute()
    {
        GameManager.Instance.taskManager.ShowTaskUI();
        yield break;
    }
}

