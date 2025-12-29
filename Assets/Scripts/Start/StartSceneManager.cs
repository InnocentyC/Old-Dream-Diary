using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartSceneManager : MonoBehaviour
{
    [Header("UI组件")]
    public GameObject startButton;  // 开始按钮
    public CanvasGroup canvasGroup;  // 用于淡出效果
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
    }

    private void OnStartButtonClicked()
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

        // 2. 开始淡出效果
        Debug.Log("开始淡出效果");
        
        if (transitionAnimator != null)
        {
            // 使用Animator做转场效果
            transitionAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.5f);  // 等待动画完成
        }
        else if (canvasGroup != null)
        {
            // 使用CanvasGroup做淡出效果
            yield return StartCoroutine(FadeOut());
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

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }
}