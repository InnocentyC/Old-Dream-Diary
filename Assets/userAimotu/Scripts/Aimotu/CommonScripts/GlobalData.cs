using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    // 梦境 1 数据
    public static bool D1_Fish, D1_Doll, D1_Award, D1_Diary;

    // 梦境 2 数据
    public static bool[] D2_Diaries = new bool[6];

    // 重置数据（如果要重玩或回主菜单时调用）
    public static void ResetAll()
    {
        D1_Fish = D1_Doll = D1_Award = D1_Diary = false;
        D2_Diaries = new bool[6];
    }
}
