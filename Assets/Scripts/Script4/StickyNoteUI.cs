using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteUI : MonoBehaviour
{
    public GameObject notePopup;

    private bool firstTime = true;

    public void OnClick()
    {
        notePopup.SetActive(true);

        if (firstTime)
        {
            DialogueManager.instance.PlayOneLine("±ãÀûÌù¶Ô°×");
            GameManager.Instance.EnterState(GameManager.RoomState.PasswordCollecting);
            firstTime = false;
        }
    }
}
