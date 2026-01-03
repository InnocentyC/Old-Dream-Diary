using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace S6
{
  /*  public class Task_S6 : MonoBehaviour
    {
        public TextMeshProUGUI passwordTaskText;
        public TextMeshProUGUI diaryTaskText;

       
        public bool IsDiaryCollected => diaryCollected;

        private const int PASSWORD_TOTAL = 3;
        private const int DIARY_TOTAL = 1;


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
        public void CollectPassword(ItemType type)
        {
            bool changed = false;

            switch (type)
            {
                case ItemType.FishTank:
                    if (!fishCollected) { fishCollected = true; changed = true; }
                    break;
                case ItemType.Doll:
                    if (!dollCollected) { dollCollected = true; changed = true; }
                    break;
                case ItemType.Award:
                    if (!awardCollected) { awardCollected = true; changed = true; }
                    break;
            }
            UpdateTaskUI();
            if (changed)
            {
                Debug.Log($"<color=green>[Task]</color> 成功收集物件: {type}，当前总进度: {CalculateCurrentCount()}/3");
            }
        }
        private int CalculateCurrentCount()
        {
            return (fishCollected ? 1 : 0) + (dollCollected ? 1 : 0) + (awardCollected ? 1 : 0);
        }

        public void CollectDiary()
        {
            if (!diaryCollected)
            {
                diaryCollected = true;
                UpdateTaskUI();
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
            if (passwordTaskText == null || diaryTaskText == null) return;
            GameManager.RoomState currentState = GameManager.Instance.CurrentState;
            if (currentState == GameManager.RoomState.PasswordCollecting || currentState == GameManager.RoomState.AllTasksDone)
            {
                passwordTaskText.gameObject.SetActive(true); // 显示密码任务
                int passwordCount = (fishCollected ? 1 : 0) + (dollCollected ? 1 : 0) + (awardCollected ? 1 : 0);
                passwordTaskText.text = $"找到 {passwordCount} / {PASSWORD_TOTAL}位密码";
                if (passwordCount == PASSWORD_TOTAL) passwordTaskText.text += " √";


                // 梦境1 自动切状态
                if (AreAllTasksCompleted() && currentState == GameManager.RoomState.PasswordCollecting)
                {
                    GameManager.Instance.EnterState(GameManager.RoomState.AllTasksDone);
                }
            }

            // --- 新增：自动检测并切换状态 ---
            if (AreAllTasksCompleted() && GameManager.Instance.CurrentState == GameManager.RoomState.PasswordCollecting)
            {
                Debug.Log("<color=lime>[Final Fix]</color> 任务数据已满，强制切换至 AllTasksDone");
                GameManager.Instance.EnterState(GameManager.RoomState.AllTasksDone);
            }
        }
        public bool AreAllTasksCompleted()
        {
            return fishCollected && dollCollected && awardCollected && diaryCollected;
        }
        public bool IsAllCompleted() => fishCollected && dollCollected && awardCollected && diaryCollected;
        // 在 Task_S4 类中增加这个方法，作为对外的统一接口
        public void UpdateUI()
        {
            UpdateTaskUI();
        }
    }*/

}