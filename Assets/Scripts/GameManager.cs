using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI组件")]
    public Image transitionPanel; // 转场黑屏

    [Header("任务进度")]
    public int passwordFound = 0;
    public int diaryFound = 0;

    // 状态锁，防止重复触发
    private bool isFishDone = false;
    private bool isDollDone = false;
    private bool isAwardDone = false;
    private bool isBeadsDone = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    // === 核心：接收物品的电话 ===
    public void HandleInteraction(ItemType type)
    {
        Debug.Log("玩家点击了：" + type); // 测试用

        switch (type)
        {
            case ItemType.Bed:
                if (passwordFound >= 3 && diaryFound >= 1)
                {
                    StartCoroutine(EndingSequence());
                }
                else
                {
                    UIManager.Instance.ShowDialogue("这里贴着什么？(也许我该找找其他线索)");
                }
                break;

            case ItemType.Note:
                UIManager.Instance.ShowItemDetail(true);
                UIManager.Instance.ShowDialogue("我画的画...好像是旧家里的景观。");
                StartCoroutine(CloseItemDelay(3f));
                break;

            case ItemType.FishTank:
                UIManager.Instance.ShowDialogue("热带鱼...一共有五条，第一个密码是 5。");
                if (!isFishDone)
                {
                    passwordFound++;
                    isFishDone = true;
                    CheckPasswordComplete();
                }
                break;

            case ItemType.Doll:
                UIManager.Instance.ShowDialogue("床上有两只玩偶...这应该是第二个密码。");
                if (!isDollDone)
                {
                    passwordFound++;
                    isDollDone = true;
                    CheckPasswordComplete();
                }
                break;

            case ItemType.Award:
                UIManager.Instance.ShowDialogue("一共 8 张奖状...这是第三个密码。");
                if (!isAwardDone)
                {
                    passwordFound++;
                    isAwardDone = true;
                    CheckPasswordComplete();
                }
                break;

            case ItemType.Beads:
                UIManager.Instance.ShowDialogue("串珠...我看见闪闪发光的未来。");
                if (!isBeadsDone)
                {
                    diaryFound++;
                    isBeadsDone = true;
                    UIManager.Instance.UpdateTaskUI(passwordFound, diaryFound);
                }
                break;
        }
    }

    // --- 辅助逻辑 ---
    void CheckPasswordComplete()
    {
        UIManager.Instance.UpdateTaskUI(passwordFound, diaryFound);
        if (passwordFound == 3) Debug.Log("密码收集完成！");
    }

    IEnumerator CloseItemDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.ShowItemDetail(false);
    }

    IEnumerator IntroSequence()
    {
        // 开场逻辑...
        yield return null;
    }

    IEnumerator EndingSequence()
    {
        UIManager.Instance.ShowDialogue("我真的很喜欢热带鱼啊...");
        yield return new WaitForSeconds(3f);
        if (transitionPanel != null) transitionPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Script5");
    }
}
