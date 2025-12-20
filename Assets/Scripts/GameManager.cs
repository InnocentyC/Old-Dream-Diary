
using UnityEngine;


public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public DialogueManager dialogueManager;
    public SceneTransitionManager sceneTransitionManager;
    public TaskManager taskManager;
    public InteractableItem interactableItem;

    public Sprite Playerportrait1;//�л���ͬ�������
    private bool allInteractionsCompleted = false;

    private bool isDiaryInteracted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �������𶯻�
      // indObjectOfType<PlayerAnimationController>().PlaySitUpAnimation();

        // �����԰�
        StartDialogueSequence();
    }

    //�����԰�----------------------------------------------------------------------
    private void StartDialogueSequence()
    {
        var openingDialogue = new DialogueSession
        {
            lines = new DialogueLine[]
            {
                new DialogueLine{speakerName ="����", text ="��Сʱ��ķ��䣿...�Һ��񻹱�С�ˡ�",portrait = Playerportrait1 }
            }
        };

        dialogueManager.StartDialogue(openingDialogue, OnDialogueFinished);
    }
    private void OnDialogueFinished()
    {
        Debug.Log("�Ի�������");
    }


    //�����߼�------------------------------------------------------------------------------
    public void OnItemInteracted(ItemType itemType)
    {
        // ����Ƿ��Ѿ��������ռǱ�
        if (!isDiaryInteracted && itemType != ItemType.Note)
        {
            Debug.Log("�㻹û���ҵ��ռǱ����޷���������Ʒ������");
            return; // ��ֹ������Ʒ�Ľ���
        }

        switch (itemType)
        {
            case ItemType.NoteBook:
                // �ҵ����뱾��һ����
                Debug.Log("�ҵ����뱾��һ���֣�");
                taskManager.FindPasswordPiece();
                break;

            case ItemType.Note:
                // �ҵ������������������ռǵ�һ���֣�
                Debug.Log("�ҵ���������");
                taskManager.FindDiary();
                break;

            case ItemType.Bed:
                // ������û������ֻ��ʾ�Ի��������߼�
                Debug.Log("��û������ֻ�Ǵ����˶Ի���");
                break;

            case ItemType.FishTank:
                // ������Ʒ�����߼�
                Debug.Log("��������׵Ľ����߼���");
                break;

            case ItemType.Doll:
                Debug.Log("����������ż�Ľ����߼���");
                break;

            case ItemType.Award:
                Debug.Log("�����˽�״�Ľ����߼���");
                break;

            case ItemType.Beads:
                Debug.Log("�����˴���Ľ����߼���");
                break;

            default:
                Debug.LogWarning($"δ��������Ʒ����: {itemType}");
                break;
        }

        // ��������Ƿ����
        CheckTasks();
    }
    private void CheckTasks()
    {
        if (taskManager.AreAllTasksCompleted())
        {
            Debug.Log("����������ɣ�׼���л�����һ������");

            // ������һ���������������л�
            sceneTransitionManager.SetNextScene("NextSceneName"); // �滻Ϊ��ĳ�������
            sceneTransitionManager.TransitionToNextScene();
        }
    }

        public void CheckInteractions()
    {
        if (AllInteractionsCompleted())
        {
            allInteractionsCompleted = true;

            
            sceneTransitionManager.TransitionToNextScene();
        }
    }

    private bool AllInteractionsCompleted()
    {
         return taskManager.AreAllTasksCompleted();
    }
}
