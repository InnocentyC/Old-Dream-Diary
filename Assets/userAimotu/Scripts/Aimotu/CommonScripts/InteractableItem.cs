using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//InteractableItem 只做三件事：
//能不能点（距离 / 状态）
//点了是什么类型
//把“请求”交给系统
    
    public class InteractableItem : MonoBehaviour
    {
        [Header("准入规则")]
        [Tooltip("只有在这些状态下，问号图标才会亮起并允许交互")]
        public List<RoomState> eligibleStates = new List<RoomState>();

        [Header("1. 基础设置")]
        public ItemType type; // 物品类型
        public bool allowRepeatClick = true;

        [Header("2. 权限与状态")]
        [SerializeField] private bool canInteract = false;
        private bool isCollected = false;
        private bool isPlayerNearby = false;
        public GameObject questionMarkIcon;

      //  private IGameManager GetManager() => FindObjectOfType<MonoBehaviour>() as IGameManager;

        [Header("4. Action 系统 (高复用性核心)")]
        public List<StateAction> defaultActions;
        [Tooltip("针对不同房间状态触发不同的动作序列")]
        public List<RoomStateEvent> stateSpecificEvents = new List<RoomStateEvent>();
        [Header("音效设置")]
        public AudioClip clickSFX; // 在 Inspector 拖入 UI_Interaction_Confirm
        private bool isExecuting = false; // 类成员变量

        private void Start()
        {
            if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
            RefreshInteractable();
        }
    // 1. 修改获取方式，直接锁定单例
        private IGameManager GetManager()
        {
            if (S4.GameManager.Instance != null) return S4.GameManager.Instance;

            // 如果单例没拿到，再尝试全局找一次
            return Object.FindAnyObjectByType<S4.GameManager>();
        }
        private void OnMouseDown()
        {
            var manager = GetManager();
            string managerStatus = (manager == null) ? "丢失(NULL)" : "正常";
            bool dialogueActive = (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive);

            Debug.Log($"<color=orange>[Interaction Check]</color> 点击物体: {gameObject.name}\n" +
                  $"1. 管理器: {managerStatus} | " +
                  $"2. UI阻断(UIBlocking): {(manager != null ? manager.IsUIBlocking.ToString() : "N/A")} | " +
                  $"3. 玩家靠近: {isPlayerNearby} | " +
                  $"4. 权限(canInteract): {canInteract} | " +
                  $"5. 执行中: {isExecuting} | " +
                  $"6. 对话中: {dialogueActive}");

            // 1. 基础拦截
            if (manager == null || !isPlayerNearby || !canInteract) return;
            //if (DialogueManager.instance.IsDialogueActive) return;
            if (!allowRepeatClick && isCollected) return;

            // --- 第二步：Action 系统优先分流 ---

            if (clickSFX != null)
            {
                manager.PlayGlobalSFX(clickSFX);
            }
            if (isExecuting || manager.IsUIBlocking) return;
            if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive) return;
            // 1. 优先检查：当前状态是否有专属 Action
            if (HasStateSpecificEvent(out List<StateAction> stateActions))
            {
                StartCoroutine(ExecuteActions(stateActions));
                return; // 匹配到专属动作，直接跳出
            }// 2. 次优先：是否有通用默认 Action
            else if (defaultActions != null && defaultActions.Count > 0)
            {
                StartCoroutine(ExecuteActions(defaultActions));
                return; // 执行默认动作，跳出
            }

        }

        // 执行 Action 序列
        private IEnumerator ExecuteActions(List<StateAction> actions)
        {
            isExecuting = true;
            foreach (var action in actions)
            {
                if (action != null)
                    yield return action.Execute();
            }
            OnInteractionFinished();
            isExecuting = false; // 结束后解锁
        }

        private void OnInteractionFinished()
        {

            if (!allowRepeatClick) isCollected = true;

            RefreshInteractable();
        }
        private bool HasStateSpecificEvent(out List<StateAction> actions)
        {
            actions = null;
            var manager = GetManager();
            if (stateSpecificEvents == null) return false;

            var currentState = manager.CurrentState;
            foreach (var ev in stateSpecificEvents)
            {
                if (ev != null && ev.triggerState == currentState)
                {
                    actions = ev.actions;
                    return true;
                }
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
                RefreshInteractable();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = false;
                RefreshInteractable();

            }
        }

        // 提供给 GameManager 调用的刷新方法
        public void RefreshInteractable()
        {
            canInteract = isPlayerNearby && CheckStateEligibility() && (!isCollected || allowRepeatClick);
            if (questionMarkIcon != null)
                questionMarkIcon.SetActive(canInteract);
        }
        private bool CheckStateEligibility()
        {
            // 如果列表为空，默认所有状态都允许（方便测试）
            if (eligibleStates == null || eligibleStates.Count == 0) return true;
            var manager = GetManager();
            if (manager == null) return false;
          
            return eligibleStates.Contains(manager.CurrentState);
        }

        // 预留的测试方法
        public void TriggerClick()
        {
            var manager = GetManager();

            // 如果没有被 UI 阻断，就执行点击逻辑
            if (manager.IsUIBlocking) return;

            Debug.Log($"[Test] 脚本触发了 {gameObject.name} 的点击");
            OnMouseDown();
        }
    }
