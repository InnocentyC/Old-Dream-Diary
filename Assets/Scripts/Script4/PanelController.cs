using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("»ù´¡ÐÐÎª")]
    [SerializeField] protected bool hideOnStart = true;

    protected virtual void Awake()
    {
        if (hideOnStart)
            gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Toggle(bool show)
    {
        gameObject.SetActive(show);
    }
}

