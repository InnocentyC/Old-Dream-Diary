using System.Collections;
using UnityEngine;
//Intro ¶Ô»°ºó ¡ú NoteLocked
//AllTasksDone ¡ú ReadyToExit

[CreateAssetMenu(menuName = "Game/State Actions/Change Room State")]
public class ChangeRoomStateAction : StateAction
{
    public GameManager.RoomState nextState;

    public override IEnumerator Execute()
    {
        GameManager.Instance.EnterState(nextState);
        yield break;
    }
}
