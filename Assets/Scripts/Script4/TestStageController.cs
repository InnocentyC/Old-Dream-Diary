using UnityEngine;

public class TestStageController : MonoBehaviour
{
    public GameManager gameManager;
    public InteractableItem notebookItem;
    public InteractableItem duckItem;
    public InteractableItem fishTankItem;
  


    void Update()
    {
        // 快捷键触发不同阶段
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("[Test] Enter Intro");
            gameManager.EnterState(GameManager.RoomState.Intro);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("[Test] Enter NoteLocked");
            gameManager.EnterState(GameManager.RoomState.NoteLocked);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("[Test] Enter PasswordCollecting");
            gameManager.EnterState(GameManager.RoomState.PasswordCollecting);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("[Test] Enter AllTasksDone");
            gameManager.EnterState(GameManager.RoomState.AllTasksDone);
        }

        // 快捷键触发物体点击
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("[Test] Click Notebook");
            notebookItem.TriggerClick();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("[Test] Click Duck");
            duckItem.TriggerClick();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("[Test] Click FishTank");
            fishTankItem.TriggerClick();
        }
    }
}
