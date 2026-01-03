using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

//简介上的贴纸
public enum StickerType
{
    None,
    Fishtank,       //鱼缸
    Doll,           //玩具熊
    Award,          //奖状
    Beads,          //鱼形串珠
    Cake,           //蛋糕
    Bubbletea,      //奶茶
    Barbecue        //蛋糕
}
public class PopupSystem : MonoBehaviour
{
    public static PopupSystem Instance;

    [Header("UI")]
    public GameObject popupPanel;
    public TextMeshProUGUI contentText;
    public Image stickerImage;
    //public Button closeButton;
    [Header("Sticker Config")]
    public Sprite defaultSticker;
    public Sprite fishtankSticker;      //鱼缸
    public Sprite dollSticker;          //玩具熊
    public Sprite awardSticker;         //奖状
    public Sprite beadsSticker;         //鱼形串珠
    public Sprite cakeSticker;          //蛋糕
    public Sprite bubbleteaSticker;     //奶茶
    public Sprite barbecueSticker;      //蛋糕

    private Action onClosedCallback;
    private bool isOpen = false; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
    }
    private void SetSticker(StickerType type)
    {
        if (stickerImage == null) return;

        Sprite target = type switch
        {
            StickerType.Fishtank => fishtankSticker,
            StickerType.Doll => dollSticker,
            StickerType.Award => awardSticker,
            StickerType.Beads => beadsSticker,
            StickerType.Cake => cakeSticker,
            StickerType.Bubbletea => bubbleteaSticker,
            StickerType.Barbecue => barbecueSticker,

            _ => defaultSticker
        };

        stickerImage.sprite = target;
        stickerImage.gameObject.SetActive(target != null);
    }


    /// <summary>
    /// 打开 Popup
    /// </summary>
    public void Open(string text, StickerType sticker, Action onClosed = null)
    {
        if (popupPanel.activeSelf)
        {
            Debug.LogWarning("Popup 已经打开，忽略重复 Open");
            return;
        }
        if (isOpen)
        {
            Debug.Log("[PopupSystem] Open 被忽略，已经在显示中");
            return;
        }
        isOpen = true; 
        popupPanel.SetActive(true);
        GameManager.Instance.PushUIBlock("Popup");
        if (contentText == null)
        {
            Debug.LogError("[PopupSystem] contentText 未绑定");
            return;
        }
        contentText.text = text.Replace("\\n", "\n");
        SetSticker(sticker);

        onClosedCallback = onClosed;

       // closeButton.onClick.RemoveAllListeners();
       // closeButton.onClick.AddListener(Close);
    }

    /// <summary>
    /// 关闭 Popup
    /// </summary>
    public void Close()
    {
        if (!isOpen) return;
        popupPanel.SetActive(false);
        GameManager.Instance.PopUIBlock("Popup");

        isOpen = false; // 标记关闭完成


        onClosedCallback?.Invoke();
        onClosedCallback = null;
    }
}
