using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartSceneManager : MonoBehaviour
{
    [Header("UI组件")]
    public Button startButton;  // 开始游戏按钮

    [Header("转场效果")]
    public GameObject transitionPanel;  // 转场黑屏
    public float transitionDuration = 1f;  // 转场时间

    void Start()
    {
        // 绑定按钮事件
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // 隐藏转场面板
        if (transitionPanel != null)
        {
            transitionPanel.SetActive(false);
        }
    }

    // 开始按钮点击事件
    public void OnStartButtonClicked()
    {
        StartCoroutine(LoadOpeningScene());
    }

    // 加载OpeningScene
    IEnumerator LoadOpeningScene()
    {
        Debug.Log("正在加载OpeningScene...");

        // 显示转场效果
        if (transitionPanel != null)
        {
            transitionPanel.SetActive(true);
        }

        // 等待转场动画
        yield return new WaitForSeconds(transitionDuration);

        // 加载目标场景
        SceneManager.LoadScene("OpeningScene");
    }

    // 清理事件监听
    void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }
}