using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 


[CreateAssetMenu(
    menuName = "Game/State Actions/Scene Transition"
)]
public class SceneTransitionAction : StateAction
{
    public UnityEngine.Object sceneAsset;
    private string sceneName;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            sceneName = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
        }
#endif
    }
    public override IEnumerator Execute()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneTransitionActionDirect] Œ¥…Ë÷√≥°æ∞");
            yield break;
        }
        SceneManager.LoadScene(sceneName);
        yield break;

        /*  var transition = GameManager.Instance.sceneTransitionManager;
          if (transition == null)
          {
              Debug.LogError("[SceneTransitionAction] SceneTransitionManager missing");
              yield break;
          }

          transition.SetNextScene(nextSceneName);
          transition.TransitionToNextScene();*/
    }
}
