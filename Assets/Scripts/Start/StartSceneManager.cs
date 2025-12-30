using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartSceneManager : MonoBehaviour
{
    [Header("UI组件")]
    public GameObject startButton;  // 开始按钮
    public GameObject fadePanel;  // 黑色遮罩面板（覆盖整个屏幕）
    public CanvasGroup fadeCanvasGroup;  // 遮罩面板的CanvasGroup（用于淡入）
    public Animator transitionAnimator;  // 转场动画（如果有）

    [Header("设置")]
    public float fadeDuration = 1.0f;  // 淡出持续时间
    public string nextSceneName = "OpeningScene";  // 下一场景名称

    private bool isTransitioning = false;

    private void Start()
    {
        // 初始化UI状态
        if (startButton != null)
        {
            Button button = startButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnStartButtonClicked);
            }
        }

        // 初始时遮罩面板完全透明
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        if (isTransitioning) return;  // 防止重复点击

        Debug.Log("点击开始按钮，准备跳转到开场场景");
        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        // 1. 禁用开始按钮
        if (startButton != null)
        {
            Button button = startButton.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }
        }

        // 2. 黑色遮罩淡入（覆盖整个屏幕）
        Debug.Log("开始淡出效果");

        if (transitionAnimator != null)
        {
            // 使用Animator做转场效果
            transitionAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.5f);  // 等待动画完成
        }
        else if (fadeCanvasGroup != null)
        {
            // 使用CanvasGroup做黑色遮罩淡入
            yield return StartCoroutine(FadeInOverlay());
        }
        else
        {
            // 没有转场效果，直接跳转
            yield return new WaitForSeconds(0.5f);
        }

        // 3. 加载下一场景
        Debug.Log($"加载场景: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeInOverlay()
    {
        // 初始时不阻挡射线，让按钮可以点击
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }

        float elapsedTime = 0f;

        // 从透明(0) 淡入到完全黑色(1)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = alpha;
            }

            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
        }
    }
}