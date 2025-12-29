using System.Collections;
using UnityEngine;

//Intro、ReadyToExit对话和旁白，状态切换提示

[CreateAssetMenu(fileName = "PlayDialogueAction", menuName = "Game/State Actions/Play Dialogue")]
public class PlayDialogueAction : StateAction
{
    public DialogueSession dialogue;
    public bool changeStateAfterDialogue = false;
    public GameManager.RoomState nextState;
    public override IEnumerator Execute()
    {
        if (dialogue == null) yield break;

        bool finished = false;

        GameManager.Instance.dialogueManager.StartDialogue(dialogue, () => finished = true);
        while (!finished)yield return null;

        if (nextState != GameManager.RoomState.None)
            GameManager.Instance.EnterState(nextState);
    }
}
