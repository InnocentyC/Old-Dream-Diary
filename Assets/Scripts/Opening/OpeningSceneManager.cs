using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class OpeningSceneManager : MonoBehaviour
{
    [Header("视频播放器")]
    public VideoPlayer videoPlayer;  // 视频播放器
    public RawImage videoScreen;  // 视频显示的RawImage

    [Header("转场效果")]
    public CanvasGroup canvasGroup;  // 用于淡出效果
    public Animator transitionAnimator;  // 转场动画（如果有）

    [Header("设置")]
    public float fadeDuration = 1.0f;  // 淡出持续时间
    public float videoFadeDelay = 0.5f;  // 视频播完后多久开始淡出
    public string nextSceneName = "Script3";  // 下一场景名称

    private bool isTransitioning = false;

    private void Start()
    {
        // 设置视频播放完成后的回调
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            
            // 确保视频准备好后自动播放
            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
            }
        }
        else
        {
            Debug.LogError("未找到VideoPlayer组件！");
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        Debug.Log("视频播放完成，准备淡出并跳转场景");
        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        // 1. 等待指定的延迟时间
        Debug.Log($"等待 {videoFadeDelay} 秒后开始淡出");
        yield return new WaitForSeconds(videoFadeDelay);

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

        // 3. 停止视频播放
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // 4. 加载下一场景
        Debug.Log($"加载场景: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = 1f;  // 从完全不透明开始
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }
            
            yield return null;
        }
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void OnDestroy()
    {
        // 清理事件监听
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}