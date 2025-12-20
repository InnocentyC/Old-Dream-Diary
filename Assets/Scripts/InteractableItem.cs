

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Bed,          //��
    NoteBook,     //���뱾
    Note,         //������
    FishTank,     //���
    Doll,         //����ż
    Award,        //��״
    Beads         //����

}


public class InteractableItem : MonoBehaviour
{
    [Header("1. ��������")]
    public ItemType type; // ��Ʒ����
    public GameObject questionMarkIcon; // ����ʱ��ʾ���ʺ�ͼ��
   
    [Header("2. �������� (��ѡ)")]
    public bool hasPopup = false;   // �Ƿ���������ͼ
    public GameObject popupPanel;       // �����������
    public string popupText = "Ĭ��������������"; // ������ Inspector �����û�̬�޸�


    [Header("3. �Ի����� (��ѡ)")]
    public bool hasDialogue = true; // �Ƿ��жԻ�
    public DialogueSession dialogueData; // �������д�Ի�����
    public Sprite popupSprite;
    private bool canInteract = false; // �Ƿ��ڽ�����Χ��

    void Start()
    {
        if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
    }

    // === ������߼� ===
    private void OnMouseDown()
    {
        // 1. ������飺�Ƿ�UI�ڵ����Ƿ��ڷ�Χ�ڣ��Ƿ����ڶԻ��У�
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!canInteract) return;
        if (DialogueManager.instance.IsDialogueActive) return;

        Debug.Log($"�������Ʒ: {type}");

        // 2. ���⴦��������ǱʼǱ���������Ҫ�������UI����������ͨ����
        if (type == ItemType.NoteBook)
        {
            // ֪ͨ GameManager ���������߼������鱾UI��
            GameManager.Instance.OnItemInteracted(type);
            return;
        }

        // 3. ͨ�����̣����� -> �Ի� -> ����֪ͨ
        if (hasPopup && popupPanel != null)
        {
            // ��ʾ�������
            popupPanel.SetActive(true);


            // ��̬�����ı�
            TextMeshProUGUI textComponent = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = popupText.Replace(@"\n", "\n"); // ��̬����
            }

            // �رհ�ť�߼�
            Button closeButton = popupPanel.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    popupPanel.SetActive(false);
                    StartConversation();
                });
            }
            /*
            // ��ʾ���壬�رպ�ִ�� StartConversation
            UIManager.Instance.ShowClue(popupSprite, () => {
                StartConversation();
            });*/
        }
        else
        {
            // û�е��壬ֱ�ӶԻ�
            StartConversation();
        }
    }

    // === �����Ի� ===
    private void StartConversation()
    {
        if (hasDialogue)
        {
            // ��ʼ�Ի����Ի���������� OnFinished
            DialogueManager.instance.StartDialogue(dialogueData, OnFinished);
        }
        else
        {
            OnFinished();
        }
    }

    // === ���̽��� ===
    private void OnFinished()
    {
        // ֪ͨ GameManager�������Ʒ�������ˣ�������������
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnItemInteracted(type);
        }
    }

    // === ��Χ��� ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            if (questionMarkIcon != null) questionMarkIcon.SetActive(false);
        }
    }
}
