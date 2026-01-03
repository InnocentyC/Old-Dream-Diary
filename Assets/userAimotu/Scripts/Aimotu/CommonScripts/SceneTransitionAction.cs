using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneTrans", menuName = "Game/State Actions/Scene Transition")]
public class SceneTransitionAction : StateAction
{
    public string targetSceneName;
    public float fadeDuration = 1.0f;
    public Vector3 playerSpawnPos; // 比如设置出现在最右侧
    public override IEnumerator Execute()
    {
        yield return SceneTransitionManager.Instance.FadeTo(targetSceneName, fadeDuration, playerSpawnPos);
    }
}
