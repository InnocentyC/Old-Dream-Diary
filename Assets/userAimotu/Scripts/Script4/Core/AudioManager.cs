using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    private Coroutine fadeCoroutine; // 用于记录当前正在进行的渐变，防止冲突
    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
    public void FadeBGMVolume(float targetVolume, float duration)
    {
        // 如果之前有正在进行的渐变，先停止它，避免音量“打架”
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(DoFade(targetVolume, duration));
    }
    private IEnumerator DoFade(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }

    // 播放瞬时音效 (复用性高)
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, volume);
    }

    // 播放背景音乐
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // BGM 淡入淡出逻辑
    public IEnumerator FadeBGM(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }
}