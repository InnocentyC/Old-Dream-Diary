using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace S6
{
   public class Task_S6 : MonoBehaviour
    {
        [Header("UI 文本引用")]
        public TextMeshProUGUI diaryTaskText; // 对应左上角 "找回 0 / 6 篇日记"   


        [Header("任务数据")]
        // 对应 6 篇日记的收集状态
        public bool[] diariesCollected = new bool[6];
        private string[] diaryNames = { "初始地图", "小吃摊", "百货大楼", "旧居", "公园", "终点" };

        private void Start()
        {
            // 必须在这里加一个延迟或初始化确保 GameManager 已经存在
            if (GameManager.Instance != null)
            {
                GameManager.OnRoomStateChanged += HandleStateChanged;
            }
        }
        private void OnDestroy()
        {
            // 记得取消注册，否则切关会报错
            GameManager.OnRoomStateChanged -= HandleStateChanged;
        }
        private void HandleStateChanged(RoomState newState)
        {
            // 每次切换状态时，强行刷一次 UI 和检查
            UpdateTaskUI();
        }
        // 修改：每个密码位独立记录，防止玩家重复点击同一个物品导致计数错误。


        public void CollectDiary(int index, string customName = "")
        {
            if (index < 0 || index >= diariesCollected.Length) return;
            if (!diariesCollected[index])
            {
                diariesCollected[index] = true;
                UpdateTaskUI();
                if (index < 0 || index >= diariesCollected.Length) return;

                if (!diariesCollected[index])
                {
                    diariesCollected[index] = true;
                    string displayName = string.IsNullOrEmpty(customName) ? diaryNames[index] : customName;

                    Debug.Log($"<color=green>[Task S6]</color> 解锁日记 {index + 1}: {displayName}");

                    // 这里可以调用你的 NotificationUI 弹出提示
                    // NotificationUI.Instance.Show($"解锁日记 {index + 1}-{displayName}");

                    UpdateTaskUI();
                }
            }
        }

        public void ShowTaskUI()
        {
            this.gameObject.SetActive(true);
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }

            UpdateTaskUI();
        }

        private void UpdateTaskUI()
        {
            if (diaryTaskText == null) return;

            RoomState currentState = GameManager.Instance.CurrentState;

            if (currentState.ToString().StartsWith("S6_") || currentState == RoomState.Dream2_Bedroom)
            {
                this.gameObject.SetActive(true);

                int currentCount = CalculateCount();
                diaryTaskText.text = $"找回 {currentCount} / {diariesCollected.Length} 篇日记";

                if (currentCount == diariesCollected.Length)
                {
                    diaryTaskText.text += " <color=yellow>√</color>";
                }
            }
        }
        private int CalculateCount()
        {
            int count = 0;
            foreach (bool b in diariesCollected)
            {
                if (b) count++;
            }
            return count;
        }

        /// <summary>
        /// 检查是否所有 6 篇日记都已找回
        /// </summary>
        public bool IsAllCompleted()
        {
            return CalculateCount() == diariesCollected.Length;
        }
    
    // 在 Task_S4 类中增加这个方法，作为对外的统一接口
        public void UpdateUI()
        {
            UpdateTaskUI();
        }
    }

}