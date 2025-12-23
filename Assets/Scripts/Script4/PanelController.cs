using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public bool hideOnStart = true;

    void Start()
    {
        if (hideOnStart)
            gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        GameManager.Instance.SetUIBlocking (true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetUIBlocking(false);
    }
}

