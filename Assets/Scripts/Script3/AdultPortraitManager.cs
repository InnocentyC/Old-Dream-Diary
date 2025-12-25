using UnityEngine;
using UnityEngine.UI;

public class AdultPortraitManager : MonoBehaviour
{
    public static AdultPortraitManager Instance;
    
    [Header("立绘显示组件")]
    public Image portraitImage;
    
    [Header("成年主角立绘")]
    public Sprite adult_neutral;    // MC_6 无表情
    public Sprite adult_tired;       // MC_1 疲惫
    public Sprite adult_confused;    // 困惑
    public Sprite adult_sad;        // 伤心
    public Sprite adult_surprised;   // 惊讶
    
    public enum AdultExpression
    {
        Neutral,    // MC_6
        Tired,     // MC_1
        Confused,
        Sad,
        Surprised
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 初始隐藏立绘
        if (portraitImage != null)
            portraitImage.gameObject.SetActive(false);
    }
    
    public void ShowPortrait(AdultExpression expression)
    {
        Sprite targetSprite = GetSprite(expression);
        
        if (portraitImage != null && targetSprite != null)
        {
            portraitImage.sprite = targetSprite;
            portraitImage.gameObject.SetActive(true);
        }
    }
    
    public void HidePortrait()
    {
        if (portraitImage != null)
            portraitImage.gameObject.SetActive(false);
    }
    
    private Sprite GetSprite(AdultExpression expression)
    {
        switch (expression)
        {
            case AdultExpression.Neutral:
                return adult_neutral;    // MC_6
            case AdultExpression.Tired:
                return adult_tired;       // MC_1
            case AdultExpression.Confused:
                return adult_confused;
            case AdultExpression.Sad:
                return adult_sad;
            case AdultExpression.Surprised:
                return adult_surprised;
            default:
                return adult_neutral;
        }
    }
    
    // 提供给DialogueManager使用的公共方法
    public Sprite GetPortraitSprite(string expressionName)
    {
        switch (expressionName.ToLower())
        {
            case "mc_6":
            case "neutral":
                return adult_neutral;
            case "mc_1":
            case "tired":
                return adult_tired;
            case "confused":
                return adult_confused;
            case "sad":
                return adult_sad;
            case "surprised":
                return adult_surprised;
            default:
                return adult_neutral;
        }
    }
}