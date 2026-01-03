using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "PlayAudioAction", menuName = "Game/State Actions/Play Audio")]
public class PlayAudioAction : StateAction
{
    [Header("音效设置")]
    public AudioClip audioClip;
    [Range(0f, 1f)] 
    public float volume = 1f;

    // 如果想要随机音效（比如鸭子每次叫声不一样），可以在这里加个数组

    public override IEnumerator Execute()
    {
        if (audioClip != null)
        {
            // 在主摄像机位置播放，确保玩家能听见

            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);

         
        }
        else
        {
            Debug.LogWarning("PlayAudioAction: 没有设置 AudioClip！");
        }

        yield return null; // 音效是瞬间触发的，不需要卡住流程
    }
}