using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class ChoiceUISystem : MonoBehaviour
{
    public static ChoiceUISystem Instance;

    [Header("UI 引用")]
    public GameObject choicePanel;      // 整个选项面板
    public TextMeshProUGUI questionText; // 问题标题
    public Transform buttonContainer;   // 按钮的父物体（建议加 Vertical Layout Group）
    public GameObject buttonPrefab;     // 选项按钮预制体

    private Action<int> onSelectionDone;

    private void Awake()
    {
        Instance = this;
        if (choicePanel) choicePanel.SetActive(false);
    }

    public void Show(string question, string[] options, Action<int> callback)
    {
        onSelectionDone = callback;
        questionText.text = question;
        choicePanel.SetActive(true);
        GameManager.Instance.PushUIBlock("ChoiceUI");

        // 清理旧按钮
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        // 生成新按钮
        for (int i = 0; i < options.Length; i++)
        {
            int index = i; // 闭包捕获
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            btnObj.GetComponent<Button>().onClick.AddListener(() => SelectOption(index));
        }
    }

    private void SelectOption(int index)
    {
        choicePanel.SetActive(false);
        GameManager.Instance.PopUIBlock("ChoiceUI");
        onSelectionDone?.Invoke(index);
    }
}