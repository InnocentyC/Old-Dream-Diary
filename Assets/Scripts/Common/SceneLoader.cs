using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("转场效果")]
    public Animator transitionAnimator;
    public CanvasGroup canvasGroup;
    public Image fadeOverlay;  // 黑色遮罩，用于淡入淡出

    [Header("设置")]
    public float fadeDuration = 1.0f;

    private bool isTransitioning = false;

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

    /// <summary>
    /// 跳转到指定场景（带淡出效果）
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        if (Instance == null)
        {
            Debug.LogError("未找到SceneLoader实例！");
            SceneManager.LoadScene(sceneName);
            return;
        }

        Instance.StartCoroutine(Instance.LoadSceneCoroutine(sceneName));
    }

    /// <summary>
    /// 跳转到指定场景（带回调）
    /// </summary>
    public static void LoadScene(string sceneName, System.Action onFadeOutComplete)
    {
        if (Instance == null)
        {
            Debug.LogError("未找到SceneLoader实例！");
            SceneManager.LoadScene(sceneName);
            onFadeOutComplete?.Invoke();
            return;
        }

        Instance.StartCoroutine(Instance.LoadSceneCoroutine(sceneName, onFadeOutComplete));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, System.Action onFadeOutComplete = null)
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        Debug.Log($"开始场景跳转: {sceneName}");

        // 1. 淡出当前场景
        yield return StartCoroutine(FadeOut());

        // 2. 触发淡出完成回调
        onFadeOutComplete?.Invoke();

        // 3. 加载新场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 4. 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 5. 淡入新场景
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
        Debug.Log($"场景加载完成: {sceneName}");
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = 0f;  // 从完全透明开始

        // 如果有遮罩图片，先设置为透明
        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0f;
            fadeOverlay.color = color;
        }

        // 淡出效果（显示黑色遮罩）
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - alpha;  // UI淡出
            }

            if (fadeOverlay != null)
            {
                Color color = fadeOverlay.color;
                color.a = alpha;
                fadeOverlay.color = color;
            }

            yield return null;
        }

        // 确保完全黑色
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 1f;
            fadeOverlay.color = color;
        }

        Debug.Log("淡出完成");
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float startAlpha = 1f;  // 从完全黑色开始

        // 淡入效果（隐藏黑色遮罩）
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - alpha;  // UI淡入
            }

            if (fadeOverlay != null)
            {
                Color color = fadeOverlay.color;
                color.a = alpha;
                fadeOverlay.color = color;
            }

            yield return null;
        }

        // 确保完全透明
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0f;
            fadeOverlay.color = color;
        }

        Debug.Log("淡入完成");
    }

    /// <summary>
    /// 获取当前是否正在转场
    /// </summary>
    public static bool IsTransitioning()
    {
        return Instance != null && Instance.isTransitioning;
    }
}