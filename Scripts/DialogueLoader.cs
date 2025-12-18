using System.IO;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public DialogueData LoadDialogueFromJson(string fileName)
    {
        // 构建文件路径
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            // 读取 JSON 文件内容
            string jsonContent = File.ReadAllText(filePath);

            // 解析 JSON 内容为 DialogueData 对象
            return JsonUtility.FromJson<DialogueData>(jsonContent);
        }
        else
        {
            Debug.LogError($"找不到 JSON 文件：{filePath}");
            return null;
        }
    }
}
