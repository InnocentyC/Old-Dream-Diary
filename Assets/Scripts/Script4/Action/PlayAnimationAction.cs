using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[CreateAssetMenu(
    menuName = "Game/State Actions/Play Animation"
)]
public class PlayAnimationAction : StateAction
{
    [Header("是否等待动画完成")]
    public bool waitForAnimation = true;

   // public GameManager.RoomState nextState = GameManager.RoomState.NoteLocked;
    public RawImage rawImage => GameManager.Instance?.uiRawImage;
    public VideoPlayer videoPlayer => GameManager.Instance?.uiVideoPlayer;
    public RenderTexture renderTexture => GameManager.Instance?.uiRenderTexture;

    public override IEnumerator Execute()
    {
        /*
        if (videoPlayer == null || rawImage == null || renderTexture == null)
        {
            Debug.LogError("VideoPlayer 或 RawImage 或 RenderTexture  未赋值！");
            yield break;
        }*/
        // 配置 VideoPlayer
        yield return new WaitUntil(() =>
           GameManager.Instance != null &&
           GameManager.Instance.uiRawImage != null &&
           GameManager.Instance.uiVideoPlayer != null &&
           GameManager.Instance.uiRenderTexture != null
       );
        var rawImage = GameManager.Instance.uiRawImage;
        var videoPlayer = GameManager.Instance.uiVideoPlayer;
        var renderTexture = GameManager.Instance.uiRenderTexture;

        // 配置 VideoPlayer
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture;

        rawImage.gameObject.SetActive(true);  // 显示 UI
        videoPlayer.Play();

        if (waitForAnimation)
        {
            while (videoPlayer.isPlaying)
            {
                yield return null;
            }
        }
        else
        {
            yield return null; // 不等待，直接执行下一步
        }

        rawImage.gameObject.SetActive(false); // 隐藏 UI
        
    }
}
