using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[CreateAssetMenu(
    menuName = "Game/State Actions/Play Animation"
)]
public class PlayAnimationAction : StateAction
{
  
    public GameManager.RoomState nextState = GameManager.RoomState.NoteLocked;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    private RenderTexture renderTexture;
    [Header("是否等待动画完成")]
    public bool waitForAnimation = true;
    public override IEnumerator Execute()
    {
        if (videoPlayer == null || rawImage == null)
        {
            Debug.LogError("VideoPlayer 或 RawImage 未赋值！");
            yield break;
        }
        rawImage.gameObject.SetActive(true);  // 显示 UI
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        rawImage.gameObject.SetActive(false); // 隐藏 UI
        
    }
}
