using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 执行转场逻辑
    /// </summary>
    public IEnumerator FadeTo(string sceneName, float duration, Vector3 spawnPos)
    {
        Debug.Log($"[SceneTransition] 准备前往: {sceneName}");

        // 这里可以接入你的 UI 遮罩动画 (Fade In)

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // 场景加载完毕后，尝试寻找玩家并定位
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.transform.position = spawnPos;

        // 这里可以接入遮罩动画 (Fade Out)
        yield return null;
    }
}
