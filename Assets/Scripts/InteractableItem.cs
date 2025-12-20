using UnityEngine;

public enum ItemType
{
    Bed,
    Note,
    FishTank,
    Doll,  //熊玩偶
    Award,
    Beads  //串珠

}

public class InteractableItem : MonoBehaviour
{
    [Header("设置")]
    public ItemType type; // 在Inspector里选它是哪种物品
    public GameObject questionMarkIcon; // 拖入头顶的“问号”小图标对象

    private bool canInteract = false; // 开关：只有靠近了才能点

    void Start()
    {
        // 游戏开始时隐藏问号
        if (questionMarkIcon != null)
            questionMarkIcon.SetActive(false);
    }

    // === 1. 鼠标点击检测 ===
    // Unity 自带方法：当鼠标点击这个物体的 Collider 时触发
    private void OnMouseDown()
    {
        Debug.Log("1. 鼠标点击到了物体：" + gameObject.name); // 如果这句话没打印，说明鼠标被UI挡住了，或者没点中Collider
        if (canInteract)
        {
            Debug.Log("2. 玩家在范围内，准备触发剧情...");

            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("3. 警告：鼠标虽然点到了物体，但由于UI遮挡，操作被拦截了！");
                return;
            }

            if (GameManager.Instance != null)
            {
                Debug.Log("4. 发送请求给 GameManager: " + type);
                GameManager.Instance.HandleInteraction(type);
            }
            else
            {
                Debug.LogError("5. 致命错误：场景里找不到 GameManager！请检查是否挂载了 GameManager 脚本。");
            }
        }
        else
        {
            Debug.Log("玩家距离太远，无法互动。");
        }
    }

    // === 2. 玩家进入范围 ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 确保是玩家碰到了
        {
            canInteract = true;
            if (questionMarkIcon != null)
                questionMarkIcon.SetActive(true); // 亮起问号
        }
    }

    // === 3. 玩家离开范围 ===
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            if (questionMarkIcon != null)
                questionMarkIcon.SetActive(false); // 熄灭问号
        }
    }

}
