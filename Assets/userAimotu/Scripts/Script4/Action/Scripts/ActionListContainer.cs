using System.Collections.Generic;
using UnityEngine;

// 这个类唯一的作用是在 ChoiceAction 里让你能看到“一组”动作列表
[System.Serializable]
public class ActionListContainer
{
    [Header("对应选项被点击后执行的动作")]
    public List<StateAction> actions;
}