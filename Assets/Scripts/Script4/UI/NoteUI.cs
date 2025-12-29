using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public GameObject canvasRoot;

    public void Open()
    {
        Debug.Log("[StickyNoteBigUI] Open");
        canvasRoot.SetActive(true);
        GameManager.Instance.PushUIBlock("Note");
    }

    public void Close()
    {
        Debug.Log("[StickyNoteBigUI] Close");
        canvasRoot.SetActive(false);
        GameManager.Instance.PopUIBlock("Note");
    }// Start is called before the first frame update
 
}
