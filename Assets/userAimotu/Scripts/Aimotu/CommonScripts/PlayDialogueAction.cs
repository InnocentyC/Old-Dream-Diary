using System.Collections;
using UnityEngine;

//Intro、ReadyToExit对话和旁白，状态切换提示

[CreateAssetMenu(fileName = "PlayDialogueAction", menuName = "Game/State Actions/Play Dialogue")]
public class PlayDialogueAction : StateAction
{
    public DialogueSession dialogue;
   // public bool changeStateAfterDialogue = false;
  //  public GameManager.RoomState nextState;
    public override IEnumerator Execute()
    {
        yield return new WaitForEndOfFrame();
        if (dialogue == null) yield break;
        if (DialogueManager.instance == null)
        {
            Debug.LogError("场景中缺少 DialogueManager!");
            yield break;
        }

        bool finished = false;

        DialogueManager.instance.StartDialogue(dialogue, () => {
            finished = true;
        });

        while (!finished)yield return null;

    }
}
