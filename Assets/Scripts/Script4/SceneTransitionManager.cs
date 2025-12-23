using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;
    private string nextSceneName; // 下一场景名称

    // 设置下一场景名称（外部调用接口）
    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }

    public void TransitionToNextScene()
    { 
        /*转场
        transitionAnimator.SetTrigger("CrumpleEffect");
        Invoke("LoadNextScene", 1.5f); // 假设动画持续 1.5 秒
        */
    }

    private void LoadNextScene()
    {
        // 加载下一场景逻辑
        SceneManager.LoadScene(nextSceneName);
    }
}

