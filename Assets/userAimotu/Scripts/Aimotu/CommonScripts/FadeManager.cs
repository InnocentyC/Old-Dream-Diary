using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public CanvasGroup fadeScreen; // 挂载黑色遮罩的 CanvasGroup
    public AudioSource musicSource; // 挂载你的背景音乐源
    public float fadeDuration = 2.0f; // 淡出持续时间（秒）
    private void Awake()
    {
        // 确保 FadeManager 在场景切换时不被销毁，这样才能完成淡入
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
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景加载完后，自动执行淡入（变亮）
        StartCoroutine(FadeIn(fadeDuration));
    }
    // 外部调用这个方法来开始转场
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }
    public IEnumerator FadeIn(float duration)
    {
        float timer = 0;
        float startVolume = musicSource != null ? musicSource.volume : 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            // 画面变亮 (Alpha 从 1 到 0)
            if (fadeScreen != null)
                fadeScreen.alpha = 1 - progress;

            // 如果需要音乐渐入，取消下面注释
            // if (musicSource != null) musicSource.volume = Mathf.Lerp(0, startVolume, progress);

            yield return null;
        }
        if (fadeScreen != null) fadeScreen.alpha = 0;
    }
    public IEnumerator FadeOut(float duration)
    {
        float timer = 0;
        float startVolume = musicSource != null ? musicSource.volume : 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            // 画面变黑 (Alpha 从 0 到 1)
            if (fadeScreen != null)
                fadeScreen.alpha = progress;

            if (musicSource != null)
                musicSource.volume = Mathf.Lerp(startVolume, 0, progress);

            yield return null;
        }
        if (fadeScreen != null) fadeScreen.alpha = 1;
    }
   
    IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return FadeOut(fadeDuration); // 直接复用上面的淡出
        SceneManager.LoadScene(sceneName);
    }
}