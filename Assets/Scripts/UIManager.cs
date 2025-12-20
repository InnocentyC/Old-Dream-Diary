
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("���� UI ���")]
    public GameObject cluePanel;   // ��������
    public Image clueImage;        // �������ͼƬ
    public Button closeButton;     // �رհ�ť

    [Header("����UI")]
    public TextMeshProUGUI passwordTaskText; // ����������ı���
    public TextMeshProUGUI diaryTaskText;    // �ռ�������ı���

    private Action onCloseCallback; // �رյ����Ҫ������

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (cluePanel != null) cluePanel.SetActive(false);

        // �󶨹رհ�ť�¼�
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCluePanel);
        }
    }
    private void Start()
    {
        // ȷ�����ر�
        if (cluePanel != null) cluePanel.SetActive(false);

        // �����رհ�ť����¼�
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                CloseCluePanel();
            });
        }
    }
    // ��ʾ����
    public void ShowClue(Sprite clueSprite, Action callback = null)
    {
        if (cluePanel != null)
        {
            cluePanel.SetActive(true);
        }

        if (clueImage != null && clueSprite != null)
        {
            clueImage.sprite = clueSprite;
        }

        onCloseCallback = callback; // ��ס�ر�ʱҪ������翪ʼ�Ի���
    }

    // �رյ���
    public void CloseCluePanel()
    {
        if (cluePanel != null && cluePanel.activeSelf)
        {
            cluePanel.SetActive(false);

            // ִ�лص���ͨ���ǿ�ʼ�Ի���
            onCloseCallback?.Invoke();
            onCloseCallback = null;
        }
    }
    // ��������UI�ķ���
    public void UpdateTaskUI(string taskDescription, bool isPasswordTask = true)
    {
        if (isPasswordTask && passwordTaskText != null)
        {
            passwordTaskText.text = taskDescription; // �������������ı�
        }
        else if (!isPasswordTask && diaryTaskText != null)
        {
            diaryTaskText.text = taskDescription; // �����ռ������ı�
        }
    }
}
